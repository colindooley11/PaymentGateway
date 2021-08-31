namespace PaymentGateway.Api.BankSimulator
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using ComponentTests;
    using Models.Web;

    public class AcquiringBankGatewayStubDelegatingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cardPaymentRequest = await request.Content.ReadFromJsonAsync<CardPaymentRequest>();
            var status = cardPaymentRequest.CardNumber switch
            {
                MagicCards.Success => "Successful",
                _ => "Declined"
            };

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new AcquiringBankResponse { Status = status })
            };
        }
    }
}