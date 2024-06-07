using TemporalExample;
using Temporalio.Api.Enums.V1;
using Temporalio.Client;
using Temporalio.Worker;
public static class Program
{
    public static async Task Main(string[] args)
    {
            var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

            using var worker2 = new TemporalWorker(
                client, // client
                new TemporalWorkerOptions(taskQueue: "my-task-queue")
                    .AddAllActivities(new IActivity2())
                    .AddWorkflow<IWorkflow2>() // Register workflow
            );

            using var tokenSource2 = new CancellationTokenSource();
            // Start the workers
            Console.WriteLine("Worker 2 started...");
            await Task.WhenAll(worker2.ExecuteAsync(tokenSource2.Token));
    }
}