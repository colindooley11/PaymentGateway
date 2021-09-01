namespace PaymentGateway.Api.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Builders;
    using Commands;
    using Microsoft.Azure.Cosmos;
    using Models.Data;
    using Query;
    using NUnit.Framework;

    public class Given_Payment_Details_When_Retrieving_A_Card
    {
        private Container _container;
        private Guid _paymentReference;

        [SetUp]
        public async Task SetUp()
        {
            _container = new CosmosBuilderFactory().Build("https://paymentgateway-cosmos-db.documents.azure.com:443/",
                "XfvKmJMieiWNmKNcWr9LTX8HU27qvjyuPbZN5f8XviPTrM5SI9bWHosAq3FEs8WR1Nhb4j4wqSwBdGxyKI5lZA==",
                "CardPaymentIntTests", "PaymentGateway.Api.IntegrationTests");

            var saveCardPaymentCommand = new SaveCardPaymentCosmosCommand(_container);
            _paymentReference = Guid.NewGuid();
            var cardPaymentData = new CardPaymentData
            {
                Id = _paymentReference,
                PaymentReference = _paymentReference,
                CardNumber = "4444333322221111",
                Amount = 50,
                Currency = "GBP",
                ExpiryMonth = 1,
                ExpiryYear = 22
            };

            await saveCardPaymentCommand.Execute(cardPaymentData);

            var getPaymentDetailsQuery  = new GetPaymentDetailsCosmosQuery(_container);

            var paymentDetailsResponse =  await getPaymentDetailsQuery.Execute(cardPaymentData.PaymentReference);

            Assert.AreEqual(cardPaymentData.ExpiryYear, paymentDetailsResponse.ExpiryYear);
            Assert.AreEqual(cardPaymentData.Amount, paymentDetailsResponse.Amount);
            Assert.AreEqual("444433******1111", paymentDetailsResponse.FirstSixLastFour);
            Assert.AreEqual(cardPaymentData.ExpiryMonth, paymentDetailsResponse.ExpiryMonth);
            Assert.AreEqual(cardPaymentData.Currency, paymentDetailsResponse.Currency);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
           await this._container.DeleteContainerAsync();
        }
    }
}