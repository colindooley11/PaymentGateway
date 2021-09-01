namespace PaymentGateway.Api.ComponentTests.InMemory
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Api.BankSimulator;
    using BankSimulator;
    using Clients;
    using Commands;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Web;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using OutOfProcess;

    public class PaymentGatewayApiCardProcessingTestsBase
    {
        protected HttpResponseMessage _result;
        protected CardPaymentRequest _card;
        protected HttpClient _client;
        protected Mock<ISaveCardPaymentCommand> _cardPaymentCommand;
        protected BankSimulatorDelegatingHandlerSpy BankSimulatorScenarioSpy;
        protected string CardNumber;

        public void An_Out_Of_Process_Payment_Gateway_Api()
        {
            _client = new OutOfProcessApiBuilder().CreateClient();
        }

        public void An_In_Process_Payment_Gateway_Api_Without_Authentication()
        {
            An_In_Process_Payment_Gateway_Api(() => new BankSimulatorScenarioBuilder(), true);
        }
        public void An_In_Process_Payment_Gateway_Api()
        {
            An_In_Process_Payment_Gateway_Api(() => new BankSimulatorScenarioBuilder(), false);
        }
        public void An_In_Process_Payment_Gateway_Api(Func<BankSimulatorScenarioBuilder> bankSimulatorScenarioBuilder, bool withoutAuthentication)
        {
            _cardPaymentCommand = new Mock<ISaveCardPaymentCommand>();
            BankSimulatorScenarioSpy = bankSimulatorScenarioBuilder().Build();
            _client = new InMemoryApiBuilder((collection =>
            {
                collection.AddSingleton(_cardPaymentCommand.Object);
                collection.AddHttpClient<AcquiringBankClient>()
                    .ConfigureHttpMessageHandlerBuilder(builder =>
                    {
                        builder.AdditionalHandlers.Clear();
                        builder.AdditionalHandlers.Add(BankSimulatorScenarioSpy);
                        builder.AdditionalHandlers.Add(new BankSimulatorStub());
                    });

            })).CreateClient();
            if (!withoutAuthentication)
            {
                _client.DefaultRequestHeaders.Add("ApiKey", "letmein");
            }
        }

        protected async Task A_201_Created_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.Created, _result.StatusCode);
        }

        protected async Task A_401_UnAuthorised_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.Unauthorized, _result.StatusCode);
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
            if (CardNumber == null)
            {
                CardNumber = MagicCards.Success;
            }
            _card = new CardPaymentRequest
            {
                CardNumber = CardNumber,
                Amount = 10m,
                CVV = 123,
                Currency = "GBP",
                ExpiryMonth = 10,
                ExpiryYear = 22,
                PaymentReference = Guid.NewGuid()
            };
        }

        protected async Task Processing_The_Card_Payment()
        {
            var json = JsonConvert.SerializeObject(_card);
            var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            _result = await _client
                 .PostAsync(new Uri("PaymentGateway/CardPayment/ProcessPayment",
                     UriKind.Relative), content)
                 .ConfigureAwait(false); ;
        }

        protected virtual void The_Response_Is_Persisted() { }

    }
}