using TemporalExample;
using Temporalio.Api.Enums.V1;
using Temporalio.Client;
using Temporalio.Worker;
public static class Program
{
    public static async Task Main(string[] args)
    {
            var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

            using var worker1 = new TemporalWorker(
                client, // client
                new TemporalWorkerOptions(taskQueue: "my-task-queue")
                    .AddAllActivities(new IActivity1())
                    .AddWorkflow<IWorkflow1>() // Register workflow
            );
            using var tokenSource1 = new CancellationTokenSource();
            // Start the workers
            await Task.WhenAll(worker1.ExecuteAsync(tokenSource1.Token));

            Console.WriteLine("Worker 1 started. Press any key to exit.");
            Console.ReadKey();
    }
}