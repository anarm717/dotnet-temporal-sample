namespace CardService.Workflows;
using Temporalio.Workflows;
using Temporalio.Common;
using CardService.Activities;
using Temporalio.Exceptions;

[Workflow]
public class TopUpWorkflow
{

    private TaskCompletionSource<bool> _activityCompletion = new TaskCompletionSource<bool>();

    [WorkflowSignal]
    public Task ApproveRequestSignal()
    {
        // Signal received, complete the waiting task
        _activityCompletion.TrySetResult(true);
        return Task.CompletedTask;
    }

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

        decimal commissionRate;
        try
        {   
            Console.WriteLine("Calling comission service");
            IReadOnlyCollection<object?> args = new object[] { cardNumber, iban, amount, workflowId };
            commissionRate = await Workflow.ExecuteActivityAsync<decimal>("GetCommission",args,
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), TaskQueue= "comission-task-queue" , RetryPolicy = retryPolicy }
            );
        }
        catch (ApplicationFailureException ex)
        {
            throw new ApplicationFailureException("Comission service not working", ex);
        }
        var totalAmount = amount * (1 + commissionRate);
        try
        {  
            Console.WriteLine("Calling limit service");
            IReadOnlyCollection<object?> args1 = new object[] { cardNumber, amount };
            await Workflow.ExecuteActivityAsync("CheckLimit",args1,
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), TaskQueue= "limit-task-queue" , RetryPolicy = retryPolicy }
            );
        }
        catch (ApplicationFailureException ex) when (ex.ErrorType == "LimitServiceErrorException")
        {
            throw new ApplicationFailureException("Limit service not working", ex);
        }

        // Wait for the signal to complete the activity
        await _activityCompletion.Task;

        try
        {   
            Console.WriteLine("Calling card service withdraw activity");
            await Workflow.ExecuteActivityAsync(
                () => CardActivities.WithdrawAsync(cardNumber,totalAmount),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10), RetryPolicy = retryPolicy }
            );
        }
        catch (ApplicationFailureException ex) when (ex.ErrorType == "CardServiceErrorException")
        {
            throw new ApplicationFailureException("Card service not working", ex);
        }

        try
        {   
            Console.WriteLine("Calling Iban service");
            IReadOnlyCollection<object?> args1 = new object[] { cardNumber, amount };
            await Workflow.ExecuteActivityAsync("TopUp",args1,
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), TaskQueue= "iban-task-queue" , RetryPolicy = retryPolicy }
            );
        }
        catch (ApplicationFailureException ex) when (ex.ErrorType == "IbanServiceErrorException")
        {
            throw new ApplicationFailureException("Iban service not working", ex);
        }

        try
        {   
            bool comission_collected = false;
            Console.WriteLine("Calling comission service collect comission activity");
            IReadOnlyCollection<object?> args = new object[] { cardNumber, amount };
            comission_collected = await Workflow.ExecuteActivityAsync<bool>("CollectCommission",args,
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5), TaskQueue= "comission-task-queue" , RetryPolicy = retryPolicy }
            );
            if (comission_collected) {
                Console.WriteLine("Comission collected");
            }
        }
        catch (ApplicationFailureException ex) when (ex.ErrorType == "ComissionServiceErrorException")
        {
            throw new ApplicationFailureException("Comission service not working", ex);
        }

        Console.WriteLine($"Started commission child Workflow");
    }
}