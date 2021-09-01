namespace PaymentGateway.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Clients;
    using Commands;
    using Mapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Web;

    [Authorize]
    [ApiController]
    [Route("PaymentGateway/[controller]")]
    public class CardPaymentController : ControllerBase
    {
        private readonly ILogger<CardPaymentController> _logger;
        private readonly AcquiringBankClient _acquiringBankClient;
        private readonly ISaveCardPaymentCommand _cardPaymentCommand;

        public CardPaymentController(ILogger<CardPaymentController> logger, AcquiringBankClient acquiringBankClient, ISaveCardPaymentCommand cardPaymentCommand)
        {
            _logger = logger;
            _acquiringBankClient = acquiringBankClient;
            _cardPaymentCommand = cardPaymentCommand;
        }

        [HttpPost]
        [Route("ProcessPayment")]
        public async Task<IActionResult> Post([FromBody] CardPaymentRequest cardPaymentRequest)
        {
            var bankResponse = await this._acquiringBankClient.ProcessPayment(cardPaymentRequest).ConfigureAwait(false);
            await this._cardPaymentCommand.Execute(CardPaymentMapper.ToCardPaymentData(cardPaymentRequest, bankResponse.Status));
            return this.Created(new Uri($"https://localhost:5001/PaymentGateway/PaymentDetails/{cardPaymentRequest.PaymentReference}"), new PaymentGatewayResponse { Status = bankResponse.Status });
        }
    }
}
