// TopUpWorkflow/Activities/LimitActivities.cs
using System.Net.Http.Json;
using Temporalio.Activities;

namespace TopUpWorkflow.Activities
{
    public class LimitActivities
    {
        [Activity]
        public static async Task CheckLimitAsync(string cardNumber, decimal amount)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5229/api/{cardNumber}");
            response.EnsureSuccessStatusCode();
            var limit = await response.Content.ReadFromJsonAsync<decimal>();

            if (limit < amount)
            {
                throw new Exception("Limit exceeded");
            }
        }
    }
}