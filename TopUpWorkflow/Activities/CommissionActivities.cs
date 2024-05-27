// TopUpWorkflow/Activities/CommissionActivities.cs
using System.Net.Http.Json;
using Temporalio.Activities;

namespace TopUpWorkflow.Activities
{
    public class CommissionActivities
    {
        [Activity]
        public static async Task<decimal> GetCommissionAsync(string cardNumber)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5236/api/{cardNumber}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<decimal>();
        }

        [Activity]
        public static async Task CollectCommissionAsync(string cardNumber, decimal amount)
        {
            // Implement commission collection logic
        }
    }
}