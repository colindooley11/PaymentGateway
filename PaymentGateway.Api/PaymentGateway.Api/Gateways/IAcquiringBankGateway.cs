namespace PaymentGateway.Api.Gateways
{
    using System.Threading.Tasks;
    using Models;

    public interface IAcquiringBankGateway
    {
        Task<AcquiringBankResponse> CapturePayment(CardPayment cardPayment);
    }
}