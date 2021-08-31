namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(
        AsA = "A merchant",
        IWant = "I want to process valid card payments",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiValidationTests : PaymentGatewayApiCardProcessingTestsBase
    {
        [Test]
        public void WhenAPaymentIsSuccessful()
        {
            this.Given(s => s.A_Payment_Gateway_Api())
                .And(s=> s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_201_Created_Is_Returned())
                .BDDfy();
        }

        [Test]
        public void WhenCardDetailsAreInvalid()
        {
            this.Given(s => s.A_Payment_Gateway_Api())
                .And(s => s.Invalid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.Then_A_400_Bad_Request_Is_Returned())
                .BDDfy();
        }
    }
}