namespace PaymentGateway.Api.Commands
{
    using System;
    using System.Threading.Tasks;
    using Models;
    using Models.Data;

    public interface ISaveCardPaymentCommand
    {
        Task Execute(CardPaymentData cardPaymentData);
    }
}