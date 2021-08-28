namespace PaymentGateway.Api.Commands
{
    using System.Threading.Tasks;
    using Models;

    public interface ISaveCardPaymentCommand
    {
        Task Execute(CardPayment cardPayment);
    }
}