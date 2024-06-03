namespace CommissionService.Worklows;

using Temporalio.Exceptions;
using Temporalio.Workflows;
using TopUpWorkflow.Activities;

[Workflow]
public class CheckCommissionWorkflow
{
    [WorkflowRun]
    public async Task<decimal> CheckCommissionAsync(string cardNumber, decimal amount)
    {
        decimal commissionRate;
            try
            {   
                commissionRate = await Workflow.ExecuteActivityAsync(
                    () => CommissionActivities.GetCommissionAsync(cardNumber),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
                );
                return commissionRate;
            }
            catch (ApplicationFailureException ex) when (ex.ErrorType == "ComissionServiceErrorException")
            {
                throw new ApplicationFailureException("Comission service not working", ex);
            }
    }
}