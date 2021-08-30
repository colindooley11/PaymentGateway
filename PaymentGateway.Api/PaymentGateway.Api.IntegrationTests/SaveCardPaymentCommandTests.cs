using NUnit.Framework;

namespace PaymentGateway.Api.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Builders;
    using Commands;
    using Microsoft.Azure.Cosmos;
    using Models;

    public class Given_A_Card_Payment_When_Saving_Card_Payment
    {
        private Container _container;
        private Guid _paymentReference;

        [SetUp]
        public async Task SetUp()
        {
            _container = new CosmosBuilderFactory().Build("https://paymentgateway-cosmos-db.documents.azure.com:443/",
                "XfvKmJMieiWNmKNcWr9LTX8HU27qvjyuPbZN5f8XviPTrM5SI9bWHosAq3FEs8WR1Nhb4j4wqSwBdGxyKI5lZA==",
                "CardPaymentIntTests", "PaymentGateway.Api.IntegrationTests");
            var saveCardPaymentCommand = new SaveCardPaymentCommand(_container);
            _paymentReference = Guid.NewGuid();
            var cardPaymentData = new CardPaymentData
            {
                Id = _paymentReference,
                PaymentReference = _paymentReference,
                CardNumber = "4444333322221111",
                Amount = 50,
                CVV = 123,
                Currency = "GBP",
                ExpiryMonth = 1,
                ExpiryYear = 22
            };

            await saveCardPaymentCommand.Execute(cardPaymentData);
        }

        [Test]
        public void Then_The_Card_Payment_Details_Are_Saved()
        {
            var query = this._container.GetItemLinqQueryable<CardPaymentData>(allowSynchronousQueryExecution:true)
                .Where(payment => payment.PaymentReference.ToString() == _paymentReference.ToString());
                       
            var card = query.ToList().First();
            Assert.AreEqual(card.CardNumber, "4444333322221111");
            Assert.AreEqual(card.Amount, 50);
            Assert.AreEqual(card.CVV, 123);
            Assert.AreEqual(card.Currency, "GBP");
            Assert.AreEqual(card.ExpiryMonth, 1);
            Assert.AreEqual(card.ExpiryYear, 22);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
         //   await this._container.DeleteContainerAsync();
        }
    }
}