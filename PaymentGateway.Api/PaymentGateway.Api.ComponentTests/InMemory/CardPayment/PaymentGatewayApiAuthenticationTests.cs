namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(
        AsA = "A merchant",
        IWant = "I want to be authenticated so that I can process valid card payments",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiAuthenticationTests : PaymentGatewayApiCardProcessingTestsBase
    {
        [Test]
        public void WhenAPaymentRequestIsUnAuthorised()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api_Without_Authentication())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_401_UnAuthorised_Is_Returned())
                .BDDfy();
        }
    }
}