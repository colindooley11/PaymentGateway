using System.Net;
using NUnit.Framework;
using PaymentGateway.Api.ComponentTests.InMemory;
using TestStack.BDDfy;

[Story(AsA = "As a merchant",
        IWant = "I want to make payments to acquiring banks",
        SoThat = "I can be paid for selling goods")]
public class PaymentGatewayApiAcquiringBankClientTests : PaymentGatewayApiCardProcessingTestsBase
{
    [Test]
    public void MakePaymentToGatewayWithValidCardWhichGoesToAcquiringBank()
    {
        this.Given(s => s.A_Payment_Gateway_Api())
            .And(s => s.Valid_Card_Details())
            .When(s => s.Processing_The_Card_Payment())
            .Then(s => s.The_Request_Url_For_The_Acquiring_Bank_Is_Correct())
            .Then(s => s.A_201_Created_Is_Returned())
            .BDDfy();
    }

    [Test]
    public void MakePaymentToGatewayWithBrokenAcquiringBank()
    {
        this.Given(s => s.A_Payment_Gateway_Api(true))
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
        Assert.AreEqual("https://bigbank.com/processpayment", _acquiringBankGatewaySpyDelegatingHandler.RequestUri);
    }

}
