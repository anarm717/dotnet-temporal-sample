namespace ComissionService.Workflows;
using Temporalio.Workflows;
using Temporalio.Common;
using LimitService.Activities;

[Workflow]
public class TopUpWorkflow
{
    [WorkflowRun]
    public async Task CheckComissionAsync(string cardNumber,string iban, decimal amount, string workflowId)
    {
        Console.WriteLine($"Checking commission for card {cardNumber}");
         var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(1),
            MaximumInterval = TimeSpan.FromSeconds(100),
            BackoffCoefficient = 2,
            MaximumAttempts = 2};
          await Workflow.ExecuteActivityAsync(
                    () => LimitActivities.CheckLimitAsync(cardNumber, amount),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
        );
        Console.WriteLine($"Commission check completed for card {cardNumber}");

    }

}