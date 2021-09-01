using System;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Query
{
    using Models.Web;

    public interface IGetPaymentDetailsQuery
    {
        Task<PaymentDetailsResponse> Execute(Guid paymentReference);
    }
}