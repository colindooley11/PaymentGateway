
namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using BankSimulator;
    using System.Net;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(AsA = "As a merchant",
        IWant = "I want to make payments to acquiring banks",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiAcquiringBankClientTests : PaymentGatewayApiCardProcessingTestsBase
    {
        [Test]
        public void MakePaymentToAcquiringBank()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.The_Request_Url_For_The_Acquiring_Bank_Is_Correct())
                .Then(s => s.A_201_Created_Is_Returned())
                .BDDfy();
        }

        [Test]
        public void MakePaymentToBrokenAcquiringBank()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api(() => new BankSimulatorScenarioBuilder().WithFailure()))
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_500_InternalServer_Error_Is_Returned())
                .BDDfy();
        }

        private void A_500_InternalServer_Error_Is_Returned()
        {
            Assert.AreEqual(HttpStatusCode.InternalServerError, this._result.StatusCode);
        }

        private void The_Request_Url_For_The_Acquiring_Bank_Is_Correct()
        {
            Assert.AreEqual("https://bigbank.com/processpayment", BankSimulatorScenarioSpy.RequestUri);
        }
    }
}