// TopUpWorkflow/Activities/CommissionActivities.cs
using Temporalio.Activities;

namespace CommissionService.Activities
{
    public class CommissionActivities
    {
        [Activity]
        public static async Task<decimal> GetCommissionAsync(string cardNumber)
        {
            await Task.Delay(5000);
            return 0.10M; // Example commission rate: 10%
        }

        [Activity]
        public static async Task<bool> CollectCommissionAsync(string cardNumber, decimal amount)
        {
            Console.WriteLine($"Collecting commission for card {cardNumber} and amount {amount}");
            return true;
        }
    }
}