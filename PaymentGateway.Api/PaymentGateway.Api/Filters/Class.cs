namespace PaymentGateway.Api.ValidationFilter
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Models.Web;

    public class ValidateModelFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next.Invoke().ConfigureAwait(false);
                return;
            }

            var errors = context.ModelState
                .Where(ms => ms.Value.ValidationState == ModelValidationState.Invalid)
                .SelectMany(ms => ms.Value.Errors.Select(e => (key: ms.Key, errorMessage: e.ErrorMessage)))
                .Select(error => BuildApiError(error.errorMessage, error.key));

            context.Result = new BadRequestObjectResult(errors);
        }

        private static ErrorResponse BuildApiError(string errorMessage, string parameterName)
        {
            return new ErrorResponse
            {
                Message = errorMessage,
                PropertyName = parameterName,
                ErrorMessage = errorMessage
            };
        }
    }
}