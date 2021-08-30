namespace PaymentGateway.Api.Commands
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface ISaveCardPaymentCommand
    {
        Task Execute(CardPaymentData cardPaymentData);
    }
}