using BuildingBlocks.Security.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Security
{
	public class ValidateSignatureAttribute : Attribute, IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
			var isValidationEnabled = configuration?.GetValue<bool>("SecurityValidation:Enabled") ?? true;

			var hasSkipAttribute = context.ActionDescriptor.EndpointMetadata
				.OfType<SkipSecurityValidationAttribute>().Any();

			if (!isValidationEnabled || hasSkipAttribute)
			{
				await next();
				return;
			}

			var signatureValidator = context.HttpContext.RequestServices.GetService<ISignatureValidator>();

			if (signatureValidator is null)
			{
				context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
				return;
			}

			if(signatureValidator.IsValid(context.HttpContext.Request))
			{
				await next();
			}
		}
	}
}
