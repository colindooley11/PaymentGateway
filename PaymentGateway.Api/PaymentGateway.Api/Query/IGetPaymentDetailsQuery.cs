using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Query
{
    using Models.Data;

    public interface IGetPaymentDetailsQuery
    {
        Task<CardPaymentData> Execute(Guid paymentReference);
    }
}