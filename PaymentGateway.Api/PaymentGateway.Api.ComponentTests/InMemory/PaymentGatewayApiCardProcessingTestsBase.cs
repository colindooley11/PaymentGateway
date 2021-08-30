namespace PaymentGateway.Api.ComponentTests.InMemory
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Commands;
    using Models.Web;
    using Moq;
    using NUnit.Framework;

    public class PaymentGatewayApiCardProcessingTestsBase
    {
        protected HttpResponseMessage _result;
        protected CardPaymentRequest _card;
        protected HttpClient _gatewayClient;
        protected Mock<ISaveCardPaymentCommand> _cardPaymentCommand;
        private object _category;

        [SetUp]
        public void Setup()
        {
            _category = "InProcess";
            if (TestContext.CurrentContext.Test.Properties.Count("Category") > 0)
            {
                _category = TestContext.CurrentContext.Test.Properties["Category"].First();
            }
        }

        protected async Task Then_A_200_OK_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.OK, _result.StatusCode);
        }

        protected async Task Then_A_201_Created_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.Created, _result.StatusCode);
        }

        protected void Then_A_400_Bad_Request_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, _result.StatusCode);
        }

        protected void Invalid_Card_Details()
        {
            _card = new CardPaymentRequest();
        }

        protected void Valid_Card_Details()
        {
            _card = new CardPaymentRequest
            {
                CardNumber = MagicCards.Success,
                Amount = 10m,
                CVV = 123,
                Currency = "GBP",
                ExpiryMonth = 10,
                ExpiryYear = 22,
                PaymentReference = Guid.NewGuid()
            };
        }

        protected void Given_A_Payment_Gateway_Api()
        {
            _cardPaymentCommand = new Mock<ISaveCardPaymentCommand>();
            if (_category.ToString() != "OutOfProcess")
            {
                _gatewayClient = new InProcessApplicationHost(_cardPaymentCommand.Object).CreateClient();
                return;
            }
            
            _gatewayClient = new OutOfProcessApplicationHost().CreateClient(); 
        }

        protected async Task When_Processing_The_Card_Payment()
        {
            _result = await _gatewayClient.PostAsJsonAsync("PaymentGateway/CardPayment/ProcessPayment", _card,
                options: new JsonSerializerOptions { IgnoreNullValues = false });
        }

        protected virtual void The_Response_Is_Persisted() { }

    }
}