namespace PaymentGateway.Api.Validators
{
    using FluentValidation;
    using Models.Web;

    public class CardPaymentValidator : AbstractValidator<CardPaymentRequest>
    {
        public CardPaymentValidator()
        {
            RuleFor(payment => payment.CardNumber)
                .NotEmpty(); 
        }
    }
}
