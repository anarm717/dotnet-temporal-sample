using TemporalExample;
using Temporalio.Api.Enums.V1;
using Temporalio.Client;
using Temporalio.Worker;
public static class Program
{
    public static async Task Main(string[] args)
    {
            var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

            var workflowId = $"top-up-activity-{Guid.NewGuid()}";
            IReadOnlyCollection<object?> args1  = new object[] { "card12311", "iban456", 40, workflowId };
            WorkflowOptions options = new(id: workflowId, taskQueue: "my-task-queue")
                                            {
                                                IdReusePolicy = WorkflowIdReusePolicy.TerminateIfRunning
                                            };

            await client.StartWorkflowAsync("IWorkflow1", args1, options);

            var workflowId1 = $"top-up-activity-{Guid.NewGuid()}";
            IReadOnlyCollection<object?> args2  = new object[] { "card12311", "iban456", 40, workflowId1 };
            WorkflowOptions options1 = new(id: workflowId, taskQueue: "my-task-queue")
                                            {
                                                IdReusePolicy = WorkflowIdReusePolicy.TerminateIfRunning
                                            };
                                            
            await client.StartWorkflowAsync("IWorkflow2", args2, options1);
            Console.WriteLine("Worklow added. Press any key to exit.");
            Console.ReadKey();
    }
}