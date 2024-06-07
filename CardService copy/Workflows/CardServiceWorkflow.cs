namespace CardService.Workflows;
using Temporalio.Workflows;
using Temporalio.Common;
using CardService.Activities;
using Temporalio.Worker;
using Temporalio.Client;

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
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), TaskQueue= "comission-task-queue2" , RetryPolicy = retryPolicy }
        );
        IReadOnlyCollection<object?> args = new object[] { cardNumber, iban, amount, workflowId };
        await Workflow.ExecuteActivityAsync("CheckComission",args,
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), TaskQueue= "comission-task-queue" , RetryPolicy = retryPolicy }
        );

        // Console.WriteLine("CardServiceFirstCheck executed");
        // var comissionWorkFlowId = $"comission-check-activity-{Guid.NewGuid()}";
        // ChildWorkflowOptions childWorkflowOptions = new ChildWorkflowOptions
        // {
        //     Id = comissionWorkFlowId,
        //     ParentClosePolicy = ParentClosePolicy.Abandon,
        //     TaskQueue = "comission-task-queue",
        // };
        // await Workflow.StartChildWorkflowAsync("ComissionWorkflow", new object[] { cardNumber, iban, amount, workflowId }, childWorkflowOptions);


        // Create a worker with the activity and workflow registered
        // var client = await TemporalClient.ConnectAsync(new("localhost:7233"));
        // using var worker1 = new TemporalWorker(
        //     client, // client
        //     new TemporalWorkerOptions(taskQueue: "top-up-task-queue")
        //         .AddAllActivities(typeof(TestActivities))
        //         .AddWorkflow<CardService.Workflows.TopUpWorkflow>() // Register workflow
        // );
        // worker1.ExecuteAsync(tokenSource.Token);

        Console.WriteLine($"Started commission child Workflow");
    }
}