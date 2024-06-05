// TopUpWorkflow/Activities/CardActivities.cs
using Temporalio.Activities;

namespace CardService.Activities
{
    public class CardActivities
    {
        [Activity]
        public static async Task WithdrawAsync(string cardNumber, decimal amount)
        {
            Console.WriteLine($"WithdrawAsync started for card {cardNumber} with amount {amount}");
        }
        [Activity]
        public static async Task CardServiceFirstCheck(string cardNumber, decimal amount)
        {
            Console.WriteLine($"CardServiceFirstCheck started for card {cardNumber} with amount {amount}");
        }
    }
}