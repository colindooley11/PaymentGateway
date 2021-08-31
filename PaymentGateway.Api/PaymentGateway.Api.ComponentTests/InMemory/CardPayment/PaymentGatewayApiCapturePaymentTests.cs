namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using System;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Models;
    using Models.Data;
    using Models.Web;
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
            this.Given(s => s.A_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_201_Created_Is_Returned())
                .And(s=> s.The_Response_Body_Indicates_Success())
                .And(s=> s.The_Response_Is_Persisted())
                .BDDfy();
        }

        protected override void The_Response_Is_Persisted()
        {
            this._cardPaymentCommand.Verify(command => command.Execute(It.Is<CardPaymentData>(data =>
                data.PaymentReference == _card.PaymentReference &&
                data.CardNumber == _card.CardNumber &&
                data.Id == _card.PaymentReference &&
                data.Amount == _card.Amount &&
                data.CVV == _card.CVV &&
                data.ExpiryMonth == _card.ExpiryMonth &&
                data.ExpiryYear == _card.ExpiryYear)));
        }

        private async Task The_Response_Body_Indicates_Success()
        {
            var paymentGatewayResponse =
                await _result.Content.ReadFromJsonAsync<PaymentGatewayResponse>();
            Assert.AreEqual(paymentGatewayResponse.Status, "Successful"); 
        }
    }
}

