namespace PaymentGateway.Api.ComponentTests.OutOfProcess
{
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using InMemory;
    using Models;
    using Models.Web;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(AsA = "As a merchant",
        IWant = "I want to make payments to acquiring banks",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiCapturePaymentTestsOutOfProcess : PaymentGatewayApiCardProcessingTestsBase
    {
        [Test]
        [Category("OutOfProcess")]
        public void MakePaymentToGatewayWithValidCardWhichCanBeCaptured()
        {
            this.Given(s => s.A_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_201_Created_Is_Returned())
                .And(s => s.The_Response_Body_Indicates_Success())
                .And(s => s.The_Response_Is_Persisted())
                .BDDfy();
        }

        protected override void The_Response_Is_Persisted()
        {
            // using Honey Comb testing:
            // https://engineering.atspotify.com/2018/01/11/testing-of-microservices/
            // we could get rid of the integration test saving to the Db, and treat the above fixture 
            // as a big sociable integration test, left this out for now
        }

        private async Task The_Response_Body_Indicates_Success()
        {
            var paymentGatewayResponse =
                await _result.Content.ReadFromJsonAsync<PaymentGatewayResponse>();
            Assert.AreEqual(paymentGatewayResponse.Status, "Successful"); 
        }
    }
}

