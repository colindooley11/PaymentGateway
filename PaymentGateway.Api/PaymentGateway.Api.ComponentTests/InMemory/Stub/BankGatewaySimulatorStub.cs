namespace PaymentGateway.Api.ComponentTests.InMemory.Stub
{
    using System.Threading.Tasks;
    using Gateways;
    using Models;

    public class BankGatewaySimulatorStub : IAcquiringBankGateway
    {
        public Task<AcquiringBankResponse> CapturePayment(CardPayment cardPayment)
        {
            string status = cardPayment.CardNumber switch
            {
                MagicCards.Success => "Successful",
                _ => "Declined"
            };
            return Task.FromResult(new AcquiringBankResponse { Status = status });
        }
    }
}