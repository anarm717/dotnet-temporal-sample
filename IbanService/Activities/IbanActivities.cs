// TopUpWorkflow/Activities/IbanActivities.cs
using System.Net.Http.Json;
using Temporalio.Activities;

namespace IbanService.Activities
{
    public class IbanActivities
    {
        [Activity]
        public static async Task TopUpAsync(string iban, decimal amount)
        {
            Console.WriteLine($"IbanService activity TopUp called with iban {iban} and amount {amount}");
        }
    }
}