namespace PaymentGateway.Api.Gateways
{
    using System.Threading.Tasks;
    using Models.Web;

    public interface IAcquiringBankGateway
    {
        Task<AcquiringBankResponse> CapturePayment(CardPaymentRequest cardPaymentRequest);
    }
}