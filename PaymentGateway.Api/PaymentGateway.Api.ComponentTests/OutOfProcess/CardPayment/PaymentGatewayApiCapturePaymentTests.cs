namespace PaymentGateway.Api.ComponentTests.OutOfProcess.CardPayment
{
    using System.Threading.Tasks;
    using InMemory;
    using Models.Web;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(AsA = "As a merchant",
        IWant = "I want to make payments to acquiring banks",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiCapturePaymentTestsOutOfProcess : PaymentGatewayApiCardProcessingTestsBase
    {
        [Ignore("Need to ensure API is running locally")]
        [Test]
        public void MakePaymentToGatewayWithValidCardWhichCanBeCaptured()
        {
            this.Given(s => s.An_Out_Of_Process_Payment_Gateway_Api())
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
            // we could get rid of the integration test saving to the Db in PaymentGateway.Api.IntegrationTests, and treat the above fixture 
            // as a big sociable integration test, but I've left this approach out
        }

        private async Task The_Response_Body_Indicates_Success()
        {
            var paymentGatewayResponseString = await _result.Content.ReadAsStringAsync();
            var paymentGatewayResponse = JsonConvert.DeserializeObject<PaymentGatewayResponse>(paymentGatewayResponseString);
            Assert.AreEqual(PaymentStatusEnum.Success, paymentGatewayResponse.Status); 
        }
    }
}

