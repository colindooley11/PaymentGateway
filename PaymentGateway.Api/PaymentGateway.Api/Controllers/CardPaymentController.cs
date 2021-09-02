namespace PaymentGateway.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Clients;
    using Commands;
    using Mapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Web;
    using PaymentGateway.Api.Query;
    using PaymentGateway.Api.Swagger;
    using Swashbuckle.AspNetCore.Filters;

    [Authorize]
    [ApiController]
    [Route("PaymentGateway/[controller]")]
    public class CardPaymentController : ControllerBase
    {
        private readonly ILogger<CardPaymentController> _logger;
        private readonly AcquiringBankClient _acquiringBankClient;
        private readonly ISaveCardPaymentCommand _cardPaymentCommand;
        private readonly IGetPaymentDetailsQuery _paymentDetailsQuery;

        public CardPaymentController(ILogger<CardPaymentController> logger, AcquiringBankClient acquiringBankClient, ISaveCardPaymentCommand cardPaymentCommand, IGetPaymentDetailsQuery paymentDetailsQuery)
        {
            _logger = logger;
            _acquiringBankClient = acquiringBankClient;
            _cardPaymentCommand = cardPaymentCommand;
            _paymentDetailsQuery = paymentDetailsQuery;
        }

        [HttpPost]
        [Route("ProcessPayment")]
        [SwaggerRequestExample(typeof(CardPaymentRequest), typeof(CardPaymentRequestExample))]
        [ProducesResponseType(typeof(AcquiringBankResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody] CardPaymentRequest cardPaymentRequest)
        {
            var paymentDetails = await this._paymentDetailsQuery.Execute(cardPaymentRequest.PaymentReference);
            if (paymentDetails != null)
            {
                return CreatedResult(cardPaymentRequest, paymentDetails.Status);
            }

            var bankResponse = await this._acquiringBankClient.ProcessPayment(cardPaymentRequest).ConfigureAwait(false);
            await this._cardPaymentCommand.Execute(CardPaymentMapper.ToCardPaymentData(cardPaymentRequest, bankResponse.Status));
            return CreatedResult(cardPaymentRequest, bankResponse.Status);
        }

        private CreatedResult CreatedResult(CardPaymentRequest cardPaymentRequest, PaymentStatusEnum status)
        {
            return this.Created(new Uri($"https://paymentgateway-api.azurewebsites.net//PaymentGateway/PaymentDetails/{cardPaymentRequest.PaymentReference}"), new PaymentGatewayResponse { Status = status });
        }
    }
}
