namespace PaymentGateway.Api.BankSimulator
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Models.Web;

    public class BankSimulatorStub : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cardPaymentRequest = await request.Content.ReadFromJsonAsync<CardPaymentRequest>();
            var status = cardPaymentRequest.CardNumber switch
            {
                MagicCards.Success => PaymentStatusEnum.Success,
                _ => PaymentStatusEnum.Failure
            };

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new AcquiringBankResponse { Status = status })
            };
        }
    }
}