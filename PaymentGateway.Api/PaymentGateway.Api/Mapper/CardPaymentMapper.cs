namespace PaymentGateway.Api.Mapper
{
    using Models;

    public class CardPaymentMapper
    {
        public static CardPaymentData ToCardPaymentData(CardPayment cardPayment)
        {
            return new CardPaymentData
            {
                PaymentReference = cardPayment.PaymentReference,
                Id = cardPayment.PaymentReference,
                Amount = cardPayment.Amount,
                ExpiryMonth = cardPayment.ExpiryMonth,
                ExpiryYear = cardPayment.ExpiryYear,
                CVV = cardPayment.CVV,
                CardNumber = cardPayment.CardNumber,
                Currency = cardPayment.Currency


            };

        }
    }
}