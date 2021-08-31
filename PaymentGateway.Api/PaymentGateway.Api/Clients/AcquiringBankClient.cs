namespace PaymentGateway.Api.Clients
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Models.Web;

    public class AcquiringBankClient
    {
        private readonly HttpClient _client;

        public HttpClient Client { get; }

        public AcquiringBankClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://bigbank.com");
            _client = client;
        }

        public async Task<AcquiringBankResponse> ProcessPayment(CardPaymentRequest cardPaymentRequest)
        {
            var response = await _client.PostAsJsonAsync("/processpayment", cardPaymentRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AcquiringBankResponse>();
        }
    }
}