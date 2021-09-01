namespace PaymentGateway.Api.Models.Web
{
    public class PaymentDetailsResponse
    {
        public string FirstSixLastFour { get; set; }
        public int CVV { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public PaymentStatusEnum  PaymentStatus { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
      
    }
}