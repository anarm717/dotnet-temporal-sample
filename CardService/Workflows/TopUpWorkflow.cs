namespace CardService.Workflows;
using Temporalio.Workflows;
using Temporalio.Common;
using Temporalio.Exceptions;
using CardService.Activities;
using CommissionService.Activities;
using LimitService.Activities;
using IbanService.Activities;
[Workflow]
public class TopUpWorkflow
{
    [WorkflowRun]
    public async Task TopUpAsync(string cardNumber,string iban, decimal amount)
    {
        Console.WriteLine("TopUpAsync started");
         // Retry policy
        var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(1),
            MaximumInterval = TimeSpan.FromSeconds(100),
            BackoffCoefficient = 2,
            MaximumAttempts = 2,
            NonRetryableErrorTypes = new[] { "ComissionServiceErrorException", "LimitServiceErrorException", "CardServiceErrorException", "IbanServiceErrorException" }
        };

        decimal commissionRate;
        try
        {   
            commissionRate = await Workflow.ExecuteActivityAsync(
                () => CommissionActivities.GetCommissionAsync(cardNumber),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10), RetryPolicy = retryPolicy }
            );
        }
        catch (ApplicationFailureException ex)
        {
            throw new ApplicationFailureException("Comission service not working", ex);
        }
        var totalAmount = amount * (1 + commissionRate);
        try
        {   
            await Workflow.ExecuteActivityAsync(
                () => LimitActivities.CheckLimitAsync(cardNumber,totalAmount),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10), RetryPolicy = retryPolicy }
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
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10), RetryPolicy = retryPolicy }
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
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10), RetryPolicy = retryPolicy }
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
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10), RetryPolicy = retryPolicy }
            );
        }
        catch (ApplicationFailureException ex) when (ex.ErrorType == "ComissionServiceErrorException")
        {
            throw new ApplicationFailureException("Comission service not working", ex);
        }
        Console.WriteLine($"Commission is {commissionRate}");
    }

}