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
        private CardPaymentData _cardPaymentData;

        [SetUp]
        public async Task SetUp()
        {
            _container = new CosmosBuilderFactory().Build("https://paymentgateway-cosmos-db.documents.azure.com:443/",
                "XfvKmJMieiWNmKNcWr9LTX8HU27qvjyuPbZN5f8XviPTrM5SI9bWHosAq3FEs8WR1Nhb4j4wqSwBdGxyKI5lZA==",
                "CardPaymentIntTests", "PaymentGateway.Api.IntegrationTests");

            var saveCardPaymentCommand = new SaveCardPaymentCosmosCommand(_container);
            _paymentReference = Guid.NewGuid();
            _cardPaymentData = new CardPaymentData
            {
                Id = _paymentReference,
                PaymentReference = _paymentReference,
                CardNumber = "444433******1111",
                Amount = 50,
                Currency = "GBP",
                ExpiryMonth = 1,
                ExpiryYear = 22
            };

            await saveCardPaymentCommand.Execute(_cardPaymentData);

           
        }

        [Test]
        public async Task Then_The_Card_Details_Are_Retrieved()
        {
            var getPaymentDetailsQuery = new GetPaymentDetailsCosmosQuery(_container);

            var paymentDetailsResponse = await getPaymentDetailsQuery.Execute(_cardPaymentData.PaymentReference);

            Assert.AreEqual(_cardPaymentData.ExpiryYear, paymentDetailsResponse.ExpiryYear);
            Assert.AreEqual(_cardPaymentData.Amount, paymentDetailsResponse.Amount);
            Assert.AreEqual("444433******1111", paymentDetailsResponse.CardNumber);
            Assert.AreEqual(_cardPaymentData.ExpiryMonth, paymentDetailsResponse.ExpiryMonth);
            Assert.AreEqual(_cardPaymentData.Currency, paymentDetailsResponse.Currency);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
           await this._container.DeleteContainerAsync();
        }
    }
}