namespace PaymentGateway.Api.Controllers
{
    using System.Threading.Tasks;
    using Commands;
    using Gateways;
    using Mapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Models.Web;

    [ApiController]
    [Route("PaymentGateway/[controller]")]
    public class CardPaymentController : ControllerBase
    {
        private readonly ILogger<CardPaymentController> _logger;
        private readonly IAcquiringBankGateway _acquiringBankGateway;
        private readonly ISaveCardPaymentCommand _cardPaymentCommand;

        public CardPaymentController(ILogger<CardPaymentController> logger, IAcquiringBankGateway acquiringBankGateway, ISaveCardPaymentCommand cardPaymentCommand)
        {
            _logger = logger;
            _acquiringBankGateway = acquiringBankGateway;
            _cardPaymentCommand = cardPaymentCommand;
        }

        [HttpPost]
        [Route("ProcessPayment")]
        public async Task<ActionResult> Post([FromBody] CardPaymentRequest cardPaymentRequest)
        {
            var bankResponse = await this._acquiringBankGateway.CapturePayment(cardPaymentRequest);
            if (bankResponse.Status == "Successful")
            {
                await this._cardPaymentCommand.Execute(CardPaymentMapper.ToCardPaymentData(cardPaymentRequest));
                return this.Created(string.Empty, new PaymentGatewayResponse { Status = "Successful" });
            }
            return this.Ok();
        }
    }
}
