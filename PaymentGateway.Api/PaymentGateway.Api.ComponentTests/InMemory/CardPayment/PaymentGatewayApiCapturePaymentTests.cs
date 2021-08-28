namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Models;
    using Moq;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(AsA = "As a merchant",
        IWant = "I want to make payments to acquiring banks",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiCapturePaymentTests : PaymentGatewayApiCardProcessingTestsBase
    {
        [Test]
        public void MakePaymentToGatewayWithValidCardWhichCanBeCaptured()
        {
            this.Given(s => s.Given_A_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.When_Processing_The_Card_Payment())
                .Then(s => s.Then_A_201_Created_Is_Returned())
                .And(s=> s.The_Response_Body_Indicates_Success())
                .And(s=> s.The_Response_Is_Persisted())
                .BDDfy();
        }

        private void The_Response_Is_Persisted()
        {
            this._cardPaymentCommand.Verify(command => command.Execute(It.IsAny<CardPayment>()));
        }

        private async Task The_Response_Body_Indicates_Success()
        {
            var paymentGatewayResponse =
                await _result.Content.ReadFromJsonAsync<PaymentGatewayResponse>();
            Assert.AreEqual(paymentGatewayResponse.Status, "Successful"); 
        }
    }
}

