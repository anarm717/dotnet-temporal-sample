// TopUpWorkflow/Activities/LimitActivities.cs
using System.Net.Http.Json;
using Temporalio.Activities;

namespace LimitService.Activities
{
    public class LimitActivities
    {
        [Activity]
        public static async Task CheckLimitAsync(string cardNumber, decimal amount)
        {
            var limit = 50;

            if (limit < amount)
            {
                throw new Exception("Limit exceeded");
            }
        }
    }
}