namespace PaymentGateway.Api.ComponentTests.InMemory.Stub
{
    using System.Threading.Tasks;
    using Gateways;
    using Models;
    using Models.Web;

    public class BankGatewaySimulatorStub : IAcquiringBankGateway
    {
        public Task<AcquiringBankResponse> CapturePayment(CardPaymentRequest cardPaymentRequest)
        {
            var status = cardPaymentRequest.CardNumber switch
            {
                MagicCards.Success => "Successful",
                _ => "Declined"
            };
            return Task.FromResult(new AcquiringBankResponse { Status = status });
        }
    }
}