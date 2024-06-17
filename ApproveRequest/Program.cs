using Temporalio.Client;
using Temporalio.Workflows;
public static class Program
{
    public static async Task Main(string[] args)
    {
        var client = await TemporalClient.ConnectAsync(new("localhost:7233") { Namespace = "default" });
        var workflowId = "top-up-activity-65fc9c63-2e34-40ae-af8a-75de483de53c";
        IReadOnlyCollection<object?> args1 = new object[] { "card12311", 40 };
        var workflow = client.GetWorkflowHandle(workflowId);
        await workflow.SignalAsync("ApproveRequestSignal", args1); 
        Console.WriteLine($"Started Workflow {workflowId}");  
    }
}