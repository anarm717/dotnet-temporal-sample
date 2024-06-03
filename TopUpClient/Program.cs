// TopUpClient/Program.cs
using Temporalio.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var client = await TemporalClient.ConnectAsync(new("localhost:7233") { Namespace = "default" });

        var workflowId = $"top-up-activity-{Guid.NewGuid()}";

        try
        {
            // Start the workflow
            var handle = await client.StartWorkflowAsync(
                (CardService.Workflows.TopUpWorkflow wf) => wf.TopUpAsync("card123", "iban456", 40),
                new(id: workflowId, taskQueue: "top-up-task-queue"));

            Console.WriteLine($"Started Workflow {workflowId}");

            // Await the result of the workflow
            // await handle.GetResultAsync();
            Console.WriteLine($"Workflow ended successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Workflow execution failed: {ex.Message}");
        }
    }
}