using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace PaymentGateway.Api.Query
{
    using System.Linq;
    using FluentValidation.Results;
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
            var result = this._container
                .GetItemQueryIterator<CardPaymentData>(
                    new QueryDefinition($"SELECT * FROM c WHERE c.PaymentReference = '{paymentReference}'"), null, new QueryRequestOptions {PartitionKey = new PartitionKey(paymentReference.ToString())});
            CardPaymentData retrievedCardPayment = null;

            foreach (var cardPayment in await result.ReadNextAsync())
            {
                retrievedCardPayment = cardPayment;
            }

            return retrievedCardPayment; 
        }
    }
}