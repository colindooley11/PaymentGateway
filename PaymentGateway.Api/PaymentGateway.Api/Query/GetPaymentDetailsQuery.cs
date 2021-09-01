using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace PaymentGateway.Api.Query
{
    using Models.Web;

    public class GetPaymentDetailsCosmosQuery : IGetPaymentDetailsQuery
    {
        private readonly Container _container;

        public GetPaymentDetailsCosmosQuery(Container container)
        {
            _container = container;
        }

        public Task<PaymentDetailsResponse> Execute(Guid paymentReference)
        {
            throw new NotImplementedException();
        }
    }
}