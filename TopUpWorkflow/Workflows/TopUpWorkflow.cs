// TopUpWorkflow/Workflows/TopUpWorkflow.cs
using Temporalio.Common;
using Temporalio.Exceptions;
using Temporalio.Workflows;
using TopUpWorkflow.Activities;

namespace TopUpWorkflow.Workflows
{
    [Workflow]
    public class TopUpWorkflow
    {
        [WorkflowRun]
        public async Task RunAsync(string cardNumber, string iban, decimal amount)
        {
             // Retry policy
            var retryPolicy = new RetryPolicy
            {
                InitialInterval = TimeSpan.FromSeconds(1),
                MaximumInterval = TimeSpan.FromSeconds(100),
                BackoffCoefficient = 2,
                MaximumAttempts = 500,
                NonRetryableErrorTypes = new[] { "ComissionServiceErrorException", "LimitServiceErrorException", "CardServiceErrorException", "IbanServiceErrorException" }
            };
            decimal commissionRate;
            try
            {   
                commissionRate = await Workflow.ExecuteActivityAsync(
                    () => CommissionActivities.GetCommissionAsync(cardNumber),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
                );
            }
            catch (ApplicationFailureException ex) when (ex.ErrorType == "ComissionServiceErrorException")
            {
                throw new ApplicationFailureException("Comission service not working", ex);
            }

            var totalAmount = amount * (1 + commissionRate);
            try
            {   
                await Workflow.ExecuteActivityAsync(
                    () => LimitActivities.CheckLimitAsync(cardNumber,totalAmount),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
                );
            }
            catch (ApplicationFailureException ex) when (ex.ErrorType == "LimitServiceErrorException")
            {
                throw new ApplicationFailureException("Limit service not working", ex);
            }

            try
            {   
                await Workflow.ExecuteActivityAsync(
                    () => CardActivities.WithdrawAsync(cardNumber,totalAmount),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
                );
            }
            catch (ApplicationFailureException ex) when (ex.ErrorType == "CardServiceErrorException")
            {
                throw new ApplicationFailureException("Card service not working", ex);
            }

            try
            {   
                await Workflow.ExecuteActivityAsync(
                    () => IbanActivities.TopUpAsync(iban,amount),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
                );
            }
            catch (ApplicationFailureException ex) when (ex.ErrorType == "IbanServiceErrorException")
            {
                throw new ApplicationFailureException("Iban service not working", ex);
            }

            try
            {   
                await Workflow.ExecuteActivityAsync(
                    () => CommissionActivities.CollectCommissionAsync(cardNumber, totalAmount - amount),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
                );
            }
            catch (ApplicationFailureException ex) when (ex.ErrorType == "ComissionServiceErrorException")
            {
                throw new ApplicationFailureException("Comission service not working", ex);
            }
        }
    }
}