namespace PaymentGateway.Api.Clients
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Models;
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

        public Task<AcquiringBankResponse> ProcessPayment(CardPaymentRequest cardPayment)
        {
           // _client.PostAsJsonAsync(new Uri("/bigbak"))
           return Task.FromResult(new AcquiringBankResponse());
        }
    }
}
