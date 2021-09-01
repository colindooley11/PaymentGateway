namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Api.BankSimulator;
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
        private string BankSimulatorOutcome;
        private PaymentStatusEnum ExpectedStatusOutcome;
        private string ExpectedMaskedCardNumber;

        [Test]
        public void MakePaymentToGatewayWithValidCard()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_201_Created_Is_Returned())
                .And(s => s.The_Response_Body_Indicates_The_Status_Of_The_Payment())
                .And(s => s.The_Response_Is_Persisted())
                .WithExamples(new ExampleTable("BankSimulatorOutcome", "CardNumber", "ExpectedMaskedCardNumber", "ExpectedStatusOutcome")
                {
                    {"Successful Card Capture", MagicCards.Success, MagicCards.SuccessMask, PaymentStatusEnum.Success},
                    {"Declined Card Capture", MagicCards.Decline, MagicCards.DeclineMask, PaymentStatusEnum.Failure}
                })
                .BDDfy();
        }

        private async Task The_Response_Body_Indicates_The_Status_Of_The_Payment()
        {
            var paymentGatewayResponse =
                await _result.Content.ReadFromJsonAsync<PaymentGatewayResponse>();
            Assert.AreEqual(ExpectedStatusOutcome, paymentGatewayResponse.Status);
        }

        protected override void The_Response_Is_Persisted()
        {
            this._cardPaymentCommand.Verify(command => command.Execute(It.Is<CardPaymentData>(data =>
                AssertCardPaymentDataIsMapped(data))));
        }

        private bool AssertCardPaymentDataIsMapped(CardPaymentData data)
        {
            Assert.AreEqual(_card.PaymentReference, data.PaymentReference);
            Assert.AreEqual(ExpectedMaskedCardNumber, data.CardNumber);
            Assert.AreEqual(_card.PaymentReference, data.Id);
            Assert.AreEqual(_card.Amount, data.Amount);
            Assert.AreEqual(_card.ExpiryMonth, data.ExpiryMonth);
            Assert.AreEqual(_card.ExpiryYear, data.ExpiryYear);
            Assert.AreEqual(_card.Currency, data.Currency);
            return true;
        }
    }
}

