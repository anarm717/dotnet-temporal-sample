// TopUpWorkflow/Activities/IbanActivities.cs
using System.Net.Http.Json;
using Temporalio.Activities;

namespace TopUpWorkflow.Activities
{
    public class IbanActivities
    {
        [Activity]
        public static async Task TopUpAsync(string iban, decimal amount)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("http://localhost:5124/api/topup", new { Iban = iban, Amount = amount });
            response.EnsureSuccessStatusCode();
        }
    }
}