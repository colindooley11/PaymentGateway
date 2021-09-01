namespace PaymentGateway.Api.ComponentTests.InMemory.CardPayment
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Models.Web;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [Story(
        AsA = "A merchant",
        IWant = "I want to be authenticated so that I can process valid card payments",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiAuthenticationTests : PaymentGatewayApiCardProcessingTestsBase
    {
        [Test]
        public void WhenAPaymentRequestIsAnAuthorised()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment(false))
                .Then(s => s.A_401_UnAuthorised_Is_Returned())
                .BDDfy();
        }
    }
}