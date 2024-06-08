using Temporalio.Client;
using Temporalio.Worker;
using IbanService.Activities;

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
    new TemporalWorkerOptions(taskQueue: "iban-task-queue")
    .AddAllActivities(new IbanActivities())// Register workflow
);

try
{
    Console.WriteLine("Iban service worker started");
    await worker.ExecuteAsync(tokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Worker cancelled");
}
