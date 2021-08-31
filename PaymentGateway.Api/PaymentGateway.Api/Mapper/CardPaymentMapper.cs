namespace PaymentGateway.Api.Mapper
{
    using Models.Data;
    using Models.Web;

    public class CardPaymentMapper
    {
        public static CardPaymentData ToCardPaymentData(CardPaymentRequest cardPaymentRequest)
        {
            return new CardPaymentData
            {
                PaymentReference = cardPaymentRequest.PaymentReference,
                Id = cardPaymentRequest.PaymentReference,
                Amount = cardPaymentRequest.Amount,
                ExpiryMonth = cardPaymentRequest.ExpiryMonth,
                ExpiryYear = cardPaymentRequest.ExpiryYear,
                CVV = cardPaymentRequest.CVV,
                CardNumber = cardPaymentRequest.CardNumber,
                Currency = cardPaymentRequest.Currency
            };
        }
    }
}