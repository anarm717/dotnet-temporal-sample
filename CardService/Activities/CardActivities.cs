// TopUpWorkflow/Activities/CardActivities.cs
using Temporalio.Activities;

namespace TopUpWorkflow.Activities
{
    public class CardActivities
    {
        [Activity]
        public static async Task WithdrawAsync(string cardNumber, decimal amount)
        {
            Console.WriteLine($"WithdrawAsync started for card {cardNumber} with amount {amount}");
        }
    }
}