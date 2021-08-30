namespace PaymentGateway.Api.Gateways
{
    using System.Threading.Tasks;
    using Models;
    using Models.Web;

    public class AcquiringBankGatewayNoOp : IAcquiringBankGateway
    {
        private readonly string _authenticationKey;

        public AcquiringBankGatewayNoOp(string authenticationKey)
        {
            _authenticationKey = authenticationKey;
        }
        public Task<AcquiringBankResponse> CapturePayment(CardPaymentRequest cardPaymentRequest)
        {
            var status = cardPaymentRequest.CardNumber switch
            {
                "4444333322221111" => "Successful",
                _ => "Declined"
            };
            return Task.FromResult(new AcquiringBankResponse { Status = status });
        }
    }
}