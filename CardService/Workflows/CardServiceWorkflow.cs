namespace CardService.Workflows;
using Temporalio.Workflows;
using Temporalio.Common;
using CardService.Activities;

[Workflow]
public class TopUpWorkflow
{
    [WorkflowRun]
    public async Task TopUpAsync(string cardNumber,string iban, decimal amount, string workflowId)
    {
        Console.WriteLine("TopUpAsync started");
        var retryPolicy = new RetryPolicy
        {
            InitialInterval = TimeSpan.FromSeconds(1),
            MaximumInterval = TimeSpan.FromSeconds(100),
            BackoffCoefficient = 2,
            MaximumAttempts = 2,
            NonRetryableErrorTypes = new[] { "ComissionServiceErrorException", "LimitServiceErrorException", "CardServiceErrorException", "IbanServiceErrorException" }
        };
        //log with params
        Console.WriteLine($"TopUpAsync started with cardNumber: {cardNumber}, iban: {iban}, amount: {amount}, workflowId: {workflowId}");
        await Workflow.ExecuteActivityAsync(
            () => CardActivities.CardServiceFirstCheck(cardNumber,amount),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), RetryPolicy = retryPolicy }
        );
        Console.WriteLine("CardServiceFirstCheck executed");
        var comissionWorkFlowId = $"comission-check-activity-{Guid.NewGuid()}";
        ChildWorkflowOptions childWorkflowOptions = new ChildWorkflowOptions
        {
            Id = comissionWorkFlowId,
            ParentClosePolicy = ParentClosePolicy.Abandon,
            TaskQueue = "comission-task-queue",
        };
        await Workflow.StartChildWorkflowAsync("ComissionWorkflow", new object[] { cardNumber, iban, amount, workflowId }, childWorkflowOptions);

        Console.WriteLine($"Started commission child Workflow");
    }
}