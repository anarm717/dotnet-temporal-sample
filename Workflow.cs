using System.Threading.Tasks;
using Temporalio.Activities;
using Temporalio.Workflows;

namespace TemporalExample
{
    public class Workflow1 : IWorkflow1
    {
        private readonly IActivity1 _activity1;

        public Workflow1(IActivity1 activity1)
        {
            _activity1 = activity1;
        }

        public async Task RunAsync()
        {
            await _activity1.DoSomethingAsync();
        }
    }

    public class Workflow2 : IWorkflow2
    {
        private readonly IActivity2 _activity2;

        public Workflow2(IActivity2 activity2)
        {
            _activity2 = activity2;
        }

        public async Task RunAsync()
        {
            await _activity2.DoSomethingElseAsync();
        }
    }

    public class Activity1 : IActivity1
    {
        public Task DoSomethingAsync()
        {
            // Activity implementation
            return Task.CompletedTask;
        }
    }

    public class Activity2 : IActivity2
    {
        public Task DoSomethingElseAsync()
        {
            // Activity implementation
            return Task.CompletedTask;
        }
    }
}