namespace CardService.Workflows;
using Temporalio.Workflows;
using CommissionService.Worklows;
using Google.Protobuf;

[Workflow]
public class TopUpWorkflow
{
    [WorkflowRun]
    public async Task TopUpAsync(string cardNumber, decimal amount)
    {
        Console.WriteLine("TopUpAsync started");
        var comission = await Workflow.ExecuteChildWorkflowAsync((CheckCommissionWorkflow wf) => wf.CheckCommissionAsync("card123", 40));
        Console.WriteLine($"Commission is {comission}");
    }

}