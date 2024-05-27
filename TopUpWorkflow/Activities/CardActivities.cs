// TopUpWorkflow/Activities/CardActivities.cs
using System.Net.Http.Json;
using Temporalio.Activities;

namespace TopUpWorkflow.Activities
{
    public class CardActivities
    {
        [Activity]
        public static async Task WithdrawAsync(string cardNumber, decimal amount)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("http://localhost:5059/api/withdraw", new { CardNumber = cardNumber, Amount = amount });
            response.EnsureSuccessStatusCode();
        }
    }
}