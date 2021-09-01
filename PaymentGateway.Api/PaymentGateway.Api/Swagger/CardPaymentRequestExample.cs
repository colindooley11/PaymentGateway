using System;

namespace PaymentGateway.Api.Swagger
{
    using PaymentGateway.Api.BankSimulator;
    using PaymentGateway.Api.Models.Web;
    using Swashbuckle.AspNetCore.Filters;

    public class CardPaymentRequestExample : IExamplesProvider<CardPaymentRequest>
    {
        public string Name => "Happy Path Card Success";

        public string Summary => "Use this to hit an acquiring bank";

        public CardPaymentRequest GetExamples()
        {
            return new CardPaymentRequest()
            {
                Amount = 50,
                CardNumber = MagicCards.Success,
                Currency = "GBP",
                CVV = 50,
                ExpiryMonth = 10,
                ExpiryYear = 22,
                PaymentReference = Guid.NewGuid()
            };
        }
    }
}
