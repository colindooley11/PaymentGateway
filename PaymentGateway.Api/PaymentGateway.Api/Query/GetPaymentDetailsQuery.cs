using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace PaymentGateway.Api.Query
{
    using System.Linq;
    using Models.Data;
    using Models.Web;

    public class GetPaymentDetailsCosmosQuery : IGetPaymentDetailsQuery
    {
        private readonly Container _container;

        public GetPaymentDetailsCosmosQuery(Container container)
        {
            _container = container;
        }

        public async Task<CardPaymentData> Execute(Guid paymentReference)
        {
            return this._container
                .GetItemLinqQueryable<CardPaymentData>(allowSynchronousQueryExecution:true).Where(payment => payment.PaymentReference.ToString() == paymentReference.ToString()).ToList().First();
        }
    }
}