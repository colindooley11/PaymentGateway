using NUnit.Framework;

namespace PaymentGateway.Api.UnitTests
{
    using System;
    using Mapper;
    using Models;

    public class Given_Card_Payment
    {
        private CardPaymentData _cardPaymentData;
        private CardPayment _cardPayment;

        [SetUp]
        public void When_Mapping_To_Dto()
        {
            _cardPayment = new CardPayment()
            {
                PaymentReference = Guid.NewGuid(),
                Amount = 10,
                CardNumber = "4444333322221111",
                Currency = "GBP",
                CVV = 123,
                ExpiryMonth = 12,
                ExpiryYear = 22
            };

            _cardPaymentData = CardPaymentMapper.ToCardPaymentData(_cardPayment);
        }

        [Test]
        public void Then_Payment_Reference_Is_Mapped()
        {
           Assert.AreEqual(_cardPaymentData.PaymentReference, _cardPayment.PaymentReference);
        }

        [Test]
        public void Then_Amount_Is_Mapped()
        {
            Assert.AreEqual(_cardPaymentData.Amount, _cardPayment.Amount);
        }

        [Test]
        public void Then_Currency_Is_Mapped()
        {
            Assert.AreEqual(_cardPaymentData.Currency, _cardPayment.Amount);
        }
    }
}