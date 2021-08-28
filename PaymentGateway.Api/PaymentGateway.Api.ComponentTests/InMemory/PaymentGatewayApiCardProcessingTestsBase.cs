namespace PaymentGateway.Api.ComponentTests.InMemory
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Commands;
    using Moq;
    using NUnit.Framework;

    public class PaymentGatewayApiCardProcessingTestsBase
    {
        protected HttpResponseMessage _result;
        protected Models.CardPayment _card;
        protected HttpClient _gatewayClient;
        protected Mock<ISaveCardPaymentCommand> _cardPaymentCommand;

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
            _card = new Models.CardPayment();
        }

        protected void Valid_Card_Details()
        {
            _card = new Models.CardPayment
            {
                CardNumber = MagicCards.Success
            };
        }

        protected void Given_A_Payment_Gateway_Api()
        {
            _cardPaymentCommand = new Mock<ISaveCardPaymentCommand>();
            _gatewayClient = new InProcessWebApplicationHost(_cardPaymentCommand.Object).CreateClient();
        }

        protected async Task When_Processing_The_Card_Payment()
        {
            _result = await _gatewayClient.PostAsJsonAsync("PaymentGateway/CardPayment/ProcessPayment", _card,
                options: new JsonSerializerOptions { IgnoreNullValues = false });
        }
    }
}