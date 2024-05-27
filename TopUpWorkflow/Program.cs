// TopUpWorkflow/Program.cs
using Temporalio.Client;
using Temporalio.Worker;
using TopUpWorkflow.Activities;
using TopUpWorkflow.Workflows;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Create a client to connect to localhost on "default" namespace
        var client = await TemporalClient.ConnectAsync(new("localhost:7233"));

        // Cancellation token to shutdown worker on ctrl+c
        using var tokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            tokenSource.Cancel();
            eventArgs.Cancel = true;
        };

        // Create a worker with the activity and workflow registered
        using var worker = new TemporalWorker(
            client, // client
            new TemporalWorkerOptions(taskQueue: "top-up-task-queue")
                .AddAllActivities(new CardActivities())
                .AddAllActivities(new CommissionActivities())
                .AddAllActivities(new LimitActivities())
                .AddAllActivities(new IbanActivities())
                .AddWorkflow<TopUpWorkflow.Workflows.TopUpWorkflow>() // Register workflow
        );

        try
        {
            await worker.ExecuteAsync(tokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Worker cancelled");
        }
    }
}





