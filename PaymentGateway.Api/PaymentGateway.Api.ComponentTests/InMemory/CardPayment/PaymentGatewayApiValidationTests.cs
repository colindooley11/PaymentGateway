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
        IWant = "I want to process valid card payments",
        SoThat = "I can be paid for selling goods")]
    public class PaymentGatewayApiValidationTests : PaymentGatewayApiCardProcessingTestsBase
    {
        private CardPaymentRequest InvalidCardExample = null;
        private string expectedErrorMessage = null;
        private string invalidCardPaymentProperty = null;

        [Test]
        public void WhenAPaymentIsSuccessful()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api())
                .And(s => s.Valid_Card_Details())
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_201_Created_Is_Returned())
                .BDDfy();
        }

        [Test]
        public void WhenCardDetailsAreInvalid()
        {
            this.Given(s => s.An_In_Process_Payment_Gateway_Api())
                .And(s => s.Invalid_Card_Details(this.InvalidCardExample))
                .When(s => s.Processing_The_Card_Payment())
                .Then(s => s.A_400_Bad_Request_Is_Returned())
                .And(s => s.The_Error_Response_Is_Correct(this.expectedErrorMessage))
                .WithExamples(new ExampleTable("InvalidCardPaymentProperty", "ExpectedErrorMessage", "InvalidCardExample")
                    {
                        { "Missing Payment Reference", "Payment reference must be guid and not empty", new CardPaymentRequest { CardNumber = "4444333322221111", Amount = 50, Currency = "GBP", CVV = 123, ExpiryMonth = 12, ExpiryYear = 22, PaymentReference = Guid.Empty}},
                        { "Card number too short", "Card number must be >=8 numbers long", new CardPaymentRequest { CardNumber = "23", Amount = 50, Currency = "GBP", CVV = 123, ExpiryMonth = 12, ExpiryYear = 22, PaymentReference = Guid.NewGuid()}},
                        { "Month not valid", "Please pass a month between 1 and 12", new CardPaymentRequest { CardNumber = "4444333322221111", Amount = 50, Currency = "GBP", CVV = 123, ExpiryMonth = 15, ExpiryYear = 22, PaymentReference = Guid.NewGuid()}},
                        { "Month not valid", "Please pass a month between 1 and 12", new CardPaymentRequest { CardNumber = "4444333322221111", Amount = 50, Currency = "GBP", CVV = 123, ExpiryMonth = 0, ExpiryYear = 22, PaymentReference = Guid.NewGuid()}},
                        { "Year not valid", "Please pass a 2 digit year between 18 and 30", new CardPaymentRequest { CardNumber = "4444333322221111", Amount = 50, Currency = "GBP", CVV = 123, ExpiryMonth = 12, ExpiryYear = 10, PaymentReference = Guid.NewGuid()}},
                        { "Year not valid", "Please pass a 2 digit year between 18 and 30", new CardPaymentRequest { CardNumber = "4444333322221111", Amount = 50, Currency = "GBP", CVV = 123, ExpiryMonth = 12, ExpiryYear = 45, PaymentReference = Guid.NewGuid()}
                    }})
                   .BDDfy();
        }

        private async Task The_Error_Response_Is_Correct(string expectedErrorMessage)
        {
            var errors = await _result.Content.ReadFromJsonAsync<IEnumerable<ErrorResponse>>();
            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual(expectedErrorMessage, errors.First().ErrorMessage);
        }
    }
}