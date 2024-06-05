using Temporalio.Api.Enums.V1;
using Temporalio.Client;
public static class Program
{
    public static async Task Main(string[] args)
    {
        var client = await TemporalClient.ConnectAsync(new("localhost:7233") { Namespace = "default" });
        
        var workflowId = $"top-up-activity-{Guid.NewGuid()}";
        IReadOnlyCollection<object?> args1  = new object[] { "card12311", "iban456", 40, workflowId };
        WorkflowOptions options = new(id: workflowId, taskQueue: "top-up-task-queue")
                                        {
                                            IdReusePolicy = WorkflowIdReusePolicy.TerminateIfRunning
                                        };
        await client.StartWorkflowAsync("TopUpWorkflow", args1, options);
        Console.WriteLine($"Started Workflow {workflowId}");

        
    }
}