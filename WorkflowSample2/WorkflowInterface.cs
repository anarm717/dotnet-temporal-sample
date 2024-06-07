using System.Threading.Tasks;
using Temporalio.Activities;
using Temporalio.Workflows;

namespace TemporalExample
{
    [Workflow]
    public class IWorkflow1
    {
        [WorkflowRun]
        public async Task RunAsync(){
            Console.WriteLine("Running workflow...");
            await Workflow.ExecuteActivityAsync(
            () => IActivity1.DoSomethingAsync(),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
        );
        }
    }

    [Workflow]
    public class IWorkflow2
    {
        [WorkflowRun]
        public async Task RunAsync(){
            await Workflow.ExecuteActivityAsync(
            () => IActivity2.DoSomethingElseAsync(),
            new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
            );
        }
    }

    public class IActivity1
    {
        [Activity]
        public static async Task DoSomethingAsync(){
            Console.WriteLine("Doing something...");
        }
    }

    public class IActivity2
    {
        [Activity]
        public static async Task DoSomethingElseAsync(){
            Console.WriteLine("Doing somethingelse...");
        }
    }
}