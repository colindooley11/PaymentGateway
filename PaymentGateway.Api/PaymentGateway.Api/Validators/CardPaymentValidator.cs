namespace PaymentGateway.Api.Validators
{
    using System;
    using FluentValidation;
    using Models.Web;

    public class CardPaymentValidator : AbstractValidator<CardPaymentRequest>
    {
        public CardPaymentValidator()
        {
            RuleFor(payment => payment.PaymentReference)
                .NotNull()
                .WithMessage("Payment reference can not be empty")
                .Must((_, guid) =>
                {
                    Guid.TryParse(guid.ToString(), out var parsedGuid);
                    return parsedGuid != Guid.Empty;
                }).WithMessage("Payment reference must be guid and not empty")
                ;

            RuleFor(payment => payment.CardNumber)
                .MinimumLength(8)
                .WithMessage("Card number must be >=8 numbers long")
                .MaximumLength(19)
                .WithMessage("Card number must be <= 19 numbers long");

            RuleFor(payment => payment.ExpiryMonth)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Please pass a month between 1 and 12")
                .LessThanOrEqualTo(12)
                .WithMessage("Please pass a month between 1 and 12");

            RuleFor(payment => payment.ExpiryYear).GreaterThanOrEqualTo(18).
                WithMessage("Please pass a 2 digit year between 18 and 30")
                .LessThanOrEqualTo(30)
                .WithMessage("Please pass a 2 digit year between 18 and 30");

            RuleFor(payment => payment.Currency)
                .NotEmpty()
                .WithMessage("Please pass a 3 letter currency code")
                .MaximumLength(3)
                .WithMessage("Please pass a 3 letter currency code")
                .MinimumLength(3)
                .WithMessage("Please pass a 3 letter currency code");

            RuleFor(payment => payment.Amount)
                .GreaterThan(0)
                .WithMessage("Please supply a positive amount");
        }
    }
}