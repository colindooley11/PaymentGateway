using NUnit.Framework;

namespace PaymentGateway.Api.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Models;

    public class Given_A_Card_Payment
    {
        private DocumentClient _documentClient;
        private ResourceResponse<DocumentCollection> _collection;

        [SetUp]
        public async Task When_Saving_Card_Payment()
        {
            _documentClient = new DocumentClient(
                new Uri("https://paymentgateway-cosmos-db.documents.azure.com:443/"),
                "XfvKmJMieiWNmKNcWr9LTX8HU27qvjyuPbZN5f8XviPTrM5SI9bWHosAq3FEs8WR1Nhb4j4wqSwBdGxyKI5lZA==");
            var db = await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = "CardPaymentIntTests" });
            _collection = await _documentClient.CreateDocumentCollectionIfNotExistsAsync(db.Resource.SelfLink,
                new DocumentCollection { Id = "CardPayments" });

            var saveCardPaymentCommand = new SaveCardPaymentCommand(_collection.Resource.SelfLink, _documentClient);
            var cardPayment = new CardPayment
            {
                CardNumber = "4444333322221111",
                Amount = 50,
                CVV = 123,
                Currency = "GBP",
                ExpiryMonth = 1,
                ExpiryYear = 22

            };

            await saveCardPaymentCommand.Execute(cardPayment);
        }

        [Test]
        public void Then_The_Card_Payment_Details_Are_Saved()
        {
            var query = this._documentClient.CreateDocumentQuery<CardPayment>(_collection.Resource.SelfLink)
                .Where(payment => payment.CardNumber == "4444333322221111");
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
            await _documentClient.DeleteDocumentCollectionAsync(_collection.Resource.SelfLink);
        }
    }
}