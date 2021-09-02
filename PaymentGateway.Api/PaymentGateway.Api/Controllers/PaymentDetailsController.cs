namespace PaymentGateway.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Mapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using PaymentGateway.Api.Models.Web;
    using Query;

    [Authorize]
    [Route("PaymentGateway/[controller]")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly IGetPaymentDetailsQuery _getPaymentDetailsCosmosQuery;

        public PaymentDetailsController(IGetPaymentDetailsQuery getPaymentDetailsCosmosQuery)
        {
            _getPaymentDetailsCosmosQuery = getPaymentDetailsCosmosQuery;
        }

        [HttpGet]
        [Route("{paymentReference:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentDetailsResponse))]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrievePaymentDetails(Guid paymentReference)
        {
            var paymentDetails = await _getPaymentDetailsCosmosQuery.Execute(paymentReference);
            if (paymentDetails == null)
            {
                return new NotFoundResult();
            }

            var paymentDetailsResponse = CardPaymentMapper.ToPaymentResponse(paymentDetails);
            return new OkObjectResult(paymentDetailsResponse);
        }
    }
}
