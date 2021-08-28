namespace PaymentGateway.Api.Validators
{
    using FluentValidation;
    using Models;

    public class CardPaymentValidator : AbstractValidator<CardPayment>
    {
        public CardPaymentValidator()
        {
            RuleFor(payment => payment.CardNumber)
                .NotEmpty(); 
        }
    }
}
