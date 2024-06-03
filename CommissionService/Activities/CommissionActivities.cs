// TopUpWorkflow/Activities/CommissionActivities.cs
using Temporalio.Activities;

namespace TopUpWorkflow.Activities
{
    public class CommissionActivities
    {
        [Activity]
        public static async Task<decimal> GetCommissionAsync(string cardNumber)
        {
            return 0.10M; // Example commission rate: 10%
        }

        [Activity]
        public static async Task CollectCommissionAsync(string cardNumber, decimal amount)
        {
            // Implement commission collection logic
        }
    }
}