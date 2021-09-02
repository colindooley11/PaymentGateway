namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using System.Threading.Tasks;
    using Api.BankSimulator;
    using Models.Data;
    using Models.Web;
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(AsA = "As a merchant",
        IWant = "I want to make payments to acquiring banks",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiCapturePaymentTests : PaymentGatewayApiCardProcessingTestsBase
    {
        /// <summary>
        /// Fields used by BDDfy
        /// </summary>
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
                .Then(s => s.Location_Header_Is_Set())
                .And(s => s.The_Response_Body_Indicates_The_Status_Of_The_Payment())
                .And(s => s.The_Response_Is_Persisted())
                .WithExamples(new ExampleTable("BankSimulatorOutcome", "CardNumber", "ExpectedMaskedCardNumber", "ExpectedStatusOutcome")
                {
                    {"Successful Card Capture", MagicCards.Success, MagicCards.SuccessMask, PaymentStatusEnum.Success},
                    {"Declined Card Capture", MagicCards.Decline, MagicCards.DeclineMask, PaymentStatusEnum.Failure}
                })
                .BDDfy();
        }

        [Test]
        public void MakePaymentToGatewayWithValidCardMoreThanOnce()
        {
            this.Given(s => s.The_Result_Is_Already_Persisted(ExpectedStatusOutcome))
                .And(s => s.An_In_Process_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_201_Created_Is_Returned())
                .Then(s => s.Location_Header_Is_Set())
                .And(s => s.The_Response_Body_Indicates_The_Status_Of_The_Payment())
                .And(s => s.The_Response_Is_Not_Persisted())
                .WithExamples(new ExampleTable("BankSimulatorOutcome", "CardNumber", "ExpectedMaskedCardNumber", "ExpectedStatusOutcome")
                {
                    {"Successful Card Capture", MagicCards.Success, MagicCards.SuccessMask, PaymentStatusEnum.Success},
                    {"Declined Card Capture", MagicCards.Decline, MagicCards.DeclineMask, PaymentStatusEnum.Failure}
                })
                .BDDfy();
        }

        private async Task The_Response_Body_Indicates_The_Status_Of_The_Payment()
        {
            var paymentGatewayResponseString = await _result.Content.ReadAsStringAsync();
            var paymentGatewayResponse = JsonConvert.DeserializeObject<PaymentGatewayResponse>(paymentGatewayResponseString);
            Assert.AreEqual(ExpectedStatusOutcome, paymentGatewayResponse.Status);
        }

        protected override void The_Response_Is_Persisted()
        {
            this._cardPaymentCommand.Verify(command => command.Execute(It.Is<CardPaymentData>(data =>
                AssertCardPaymentDataIsMapped(data))));
        }

        protected void The_Response_Is_Not_Persisted()
        {
            this._cardPaymentCommand.Verify(command => command.Execute(It.IsAny<CardPaymentData>()), Times.Never);
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
            Assert.AreEqual(ExpectedStatusOutcome, data.Status);
            return true;
        }
    }
}

