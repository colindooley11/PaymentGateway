namespace PaymentGateway.Api.ComponentTests.InMemory
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Net.Mime;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using BankSimulator;
    using Commands;
    using Models.Web;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using OutOfProcess;

    public class PaymentGatewayApiCardProcessingTestsBase
    {
        private const string Outofprocess = "OutOfProcess";
        protected HttpResponseMessage _result;
        protected CardPaymentRequest _card;
        protected HttpClient _gatewayClient;
        protected Mock<ISaveCardPaymentCommand> _cardPaymentCommand;
        private object _category;
        protected AcquiringBankGatewaySpyDelegatingHandler _acquiringBankGatewaySpyDelegatingHandler;

        public void A_Payment_Gateway_Api()
        {
            A_Payment_Gateway_Api(false);
        }

        public void A_Payment_Gateway_Api(bool isBroken)
        {
            _category = "InProcess";
            if (TestContext.CurrentContext.Test.Properties.Count("Category") > 0)
            {
                _category = TestContext.CurrentContext.Test.Properties["Category"].First();
            }

            _cardPaymentCommand = new Mock<ISaveCardPaymentCommand>();
            if (_category.ToString() != Outofprocess)
            {
                _acquiringBankGatewaySpyDelegatingHandler = new AcquiringBankGatewaySpyDelegatingHandler(isBroken);
                _gatewayClient = new InProcessApplicationHost(_cardPaymentCommand.Object, _acquiringBankGatewaySpyDelegatingHandler).CreateClient();
                return;
            }

            _gatewayClient = new OutOfProcessApplicationHost().CreateClient();
        }

        protected async Task Then_A_200_OK_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.OK, _result.StatusCode);
        }

        protected async Task A_201_Created_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.Created, _result.StatusCode);
        }

        protected void A_400_Bad_Request_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, _result.StatusCode);
        }

        protected void Invalid_Card_Details(CardPaymentRequest cardPaymentRequest)
        {
            _card = cardPaymentRequest;
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

        protected void Given_A_Payment_Gateway_Api_With_Bank_Gateway_Which_Is_Down()
        {

        }

        protected void And_A_Well_Behaved_Acquiring_Bank()
        {
            _acquiringBankGatewaySpyDelegatingHandler = new AcquiringBankGatewaySpyDelegatingHandler();
        }

        protected void And_An_Acquiring_Bank_Which_Is_Broken()
        {
            _acquiringBankGatewaySpyDelegatingHandler = new AcquiringBankGatewaySpyDelegatingHandler();
        }

        protected async Task Processing_The_Card_Payment()
        {
           // _result = await _gatewayClient.PostAsJsonAsync("PaymentGateway/CardPayment/ProcessPayment", _card,
           //     options: new JsonSerializerOptions { IgnoreNullValues = false })

           var json = JsonConvert.SerializeObject(_card);
               _result = await _gatewayClient
                    .PostAsync(new Uri("PaymentGateway/CardPayment/ProcessPayment",
                        UriKind.Relative), new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json))
                    .ConfigureAwait(false); ;
        }

        protected virtual void The_Response_Is_Persisted() { }

    }
}