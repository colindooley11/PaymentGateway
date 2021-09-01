namespace PaymentGateway.Api.ComponentTests.InMemory.PaymentDetails
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Data;
    using Models.Web;
    using Moq;
    using NUnit.Framework;
    using Query;

    [TestFixture]
    public class PaymentGatewayApiGetPaymentDetailsTests
    {
        [TestFixture]
        public class Given_An_Existing_Card_Payment
        {
            private HttpClient _client;
            private HttpResponseMessage _result;
            private CardPaymentData _savedPaymentDetails;

            [SetUp]
            public async Task When_Retrieving_Payment_Details()
            {
                var paymentDetailsQuery = new Mock<IGetPaymentDetailsQuery>();
                var paymentReference = Guid.NewGuid();
                _savedPaymentDetails = new CardPaymentData
                {
                    Status = PaymentStatusEnum.Success,
                    ExpiryMonth = 12,
                    ExpiryYear = 25,
                    CardNumber = "444433******1111",
                };

                paymentDetailsQuery.Setup(query => query.Execute(It.Is<Guid>(guid => guid == paymentReference)))
                    .ReturnsAsync(() => _savedPaymentDetails);

                _client = BuildApi(paymentDetailsQuery);

                _result = await _client.GetAsync($"PaymentGateway/PaymentDetails/{paymentReference}");
            }

            [Test]
            public void Then_A_200_OK_Is_Returned()
            {
                Assert.AreEqual(HttpStatusCode.OK, _result.StatusCode);
            }

            [Test]
            public async Task Then_The_Payload_Is_Retrieved_From_The_Database()
            {
                var paymentDetailsResponse = await _result.Content.ReadFromJsonAsync<PaymentDetailsResponse>();
                Assert.AreEqual(_savedPaymentDetails.Status, paymentDetailsResponse.PaymentStatus);
                Assert.AreEqual(_savedPaymentDetails.CardNumber, paymentDetailsResponse.FirstSixLastFour);
                Assert.AreEqual(_savedPaymentDetails.ExpiryYear, paymentDetailsResponse.ExpiryYear);
                Assert.AreEqual(_savedPaymentDetails.ExpiryMonth, paymentDetailsResponse.ExpiryMonth);
            }
        }

        private static HttpClient BuildApi(Mock<IGetPaymentDetailsQuery> paymentDetailsQuery)
        {
            var _client = new InMemoryApiBuilder(collection =>
                collection.AddSingleton(paymentDetailsQuery.Object)).CreateClient();
            _client.DefaultRequestHeaders.Add("ApiKey", "letmein");
            return _client; 
        }

        [TestFixture]
        public class Given_A_Card_Payment_That_Doesnt_Exist
        {
            private HttpClient _client;
            private HttpResponseMessage _result;
      
            [Test]
            public async Task When_Retrieving_Payment_Details_Then_A_404_Not_Found_Is_Returned()
            {
                var paymentDetailsQuery = new Mock<IGetPaymentDetailsQuery>();
                paymentDetailsQuery.Setup(query => query.Execute(It.IsAny<Guid>())).ReturnsAsync(() => null);

                _client = BuildApi(paymentDetailsQuery);

                _result = await _client.GetAsync($"PaymentGateway/PaymentDetails/{Guid.NewGuid()}");

                Assert.AreEqual(HttpStatusCode.NotFound, _result.StatusCode);
            }
        }

        [TestFixture]
        public class Given_A_Database_Outage
        {
            private HttpClient _client;
            private HttpResponseMessage _result;

            [Test]
            public async Task When_Retrieving_Payment_Details_Then_A_500_Internal_Server_Error_Is_Returned()
            {
                var paymentDetailsQuery = new Mock<IGetPaymentDetailsQuery>();
                paymentDetailsQuery.Setup(query => query.Execute(It.IsAny<Guid>())).ThrowsAsync(new Exception());

                _client = BuildApi(paymentDetailsQuery);

                _result = await _client.GetAsync($"PaymentGateway/PaymentDetails/{Guid.NewGuid()}");

                Assert.AreEqual(HttpStatusCode.InternalServerError, _result.StatusCode);
            }
        }

        [TestFixture]
        public class Given_A_Request_Is_Not_Authenticated
        {
            private HttpClient _client;
            private HttpResponseMessage _result;

            [Test]
            public async Task When_Retrieving_Payment_Details_Then_A_401_Unauthorised_Error_Is_Returned()
            {
                _client = BuildApi(new Mock<IGetPaymentDetailsQuery>());
                _client.DefaultRequestHeaders.Clear();
                _result = await _client.GetAsync($"PaymentGateway/PaymentDetails/{Guid.NewGuid()}");

                Assert.AreEqual(HttpStatusCode.Unauthorized, _result.StatusCode);
            }
        }

    }
}