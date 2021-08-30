namespace PaymentGateway.Api.Gateways
{
    using System.Threading.Tasks;
    using Models;

    public class AcquiringBankGatewayNoOp : IAcquiringBankGateway
    {
        private readonly string _authenticationKey;

        public AcquiringBankGatewayNoOp(string authenticationKey)
        {
            _authenticationKey = authenticationKey;
        }
        public Task<AcquiringBankResponse> CapturePayment(CardPayment cardPayment)
        {
            var status = cardPayment.CardNumber switch
            {
                "4444333322221111" => "Successful",
                _ => "Declined"
            };
            return Task.FromResult(new AcquiringBankResponse { Status = status });
        }
    }
}