namespace PaymentGateway.Api.Mapper
{
    using System.Linq;
    using Models.Data;
    using Models.Web;

    public class CardPaymentMapper
    {
        public static CardPaymentData ToCardPaymentData(CardPaymentRequest cardPaymentRequest, PaymentStatusEnum bankResponseStatus)
        {
            return new CardPaymentData
            {
                PaymentReference = cardPaymentRequest.PaymentReference,
                Id = cardPaymentRequest.PaymentReference,
                Amount = cardPaymentRequest.Amount,
                ExpiryMonth = cardPaymentRequest.ExpiryMonth,
                ExpiryYear = cardPaymentRequest.ExpiryYear,
                CardNumber = GetFirstSixLastFour(cardPaymentRequest), 
                Currency = cardPaymentRequest.Currency,
                Status = bankResponseStatus
            };
        }


        public static PaymentDetailsResponse ToPaymentResponse(CardPaymentData cardPaymentData)
        {
            return new PaymentDetailsResponse
            {
                PaymentReference = cardPaymentData.PaymentReference,
                Amount = cardPaymentData.Amount,
                ExpiryMonth = cardPaymentData.ExpiryMonth,
                ExpiryYear = cardPaymentData.ExpiryYear,
                FirstSixLastFour = cardPaymentData.CardNumber,
                Currency = cardPaymentData.Currency,
                PaymentStatus = cardPaymentData.Status
            };
        }

        private static string GetFirstSixLastFour(CardPaymentRequest cardPaymentRequest)
        {
            return string.Concat(new string(cardPaymentRequest.CardNumber.Take(6).ToArray()), new string('*', cardPaymentRequest.CardNumber.Length - 10), new string(cardPaymentRequest.CardNumber.TakeLast(4).ToArray()));
        }
    }
}