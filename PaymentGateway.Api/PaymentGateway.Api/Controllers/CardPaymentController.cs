namespace PaymentGateway.Api.Controllers
{
    using System.Threading.Tasks;
    using Clients;
    using Commands;
    using Filters;
    using Mapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Web;

    [ApiController]
    [Route("PaymentGateway/[controller]")]
    [ApiKey]
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
            return this.Created(string.Empty, new PaymentGatewayResponse { Status = bankResponse.Status });
        }
    }
}
