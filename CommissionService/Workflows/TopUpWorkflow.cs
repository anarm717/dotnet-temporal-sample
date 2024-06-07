namespace ComissionService.Workflows;
using Temporalio.Workflows;
using Temporalio.Common;
using CommissionService.Activities;

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
        decimal commissionRate;
        commissionRate = await Workflow.ExecuteActivityAsync(
                    () => CommissionActivities.GetCommissionAsync(cardNumber),
                    new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
        );

        var limitWorkFlowId = $"limit-activity-{Guid.NewGuid()}";
        ChildWorkflowOptions childWorkflowOptions = new ChildWorkflowOptions
        {
            Id = limitWorkFlowId,
            ParentClosePolicy = ParentClosePolicy.Abandon,
            TaskQueue = "limit-task-queue",
        };
        await Workflow.StartChildWorkflowAsync("LimitWorkflow", new object[] { cardNumber, iban, amount, workflowId }, childWorkflowOptions);

        Console.WriteLine($"Started commission child Workflow");

    }

}