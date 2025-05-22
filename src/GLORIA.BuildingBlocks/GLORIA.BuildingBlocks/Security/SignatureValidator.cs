using GLORIA.BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace GLORIA.BuildingBlocks.Security
{
	public class SignatureValidator : ISignatureValidator
	{
		private readonly SecurityValidationOptions _options;
		private const string SignatureHeader = "X-Service-Signature";
		private const string ServiceNameHeader = "X-Service-Name";

		public SignatureValidator(IOptions<SecurityValidationOptions> options)
		{
			_options = options.Value;
		}

        public bool IsValid(HttpRequest request)
        {
            if (!_options.Enabled)
            {
                return true;
            }

            if (!request.Headers.TryGetValue(SignatureHeader, out var signatureHeader) ||
                !request.Headers.TryGetValue(ServiceNameHeader, out var serviceNameHeader))
            {
                throw new UnauthorizedException("Missing required headers.");
            }

            var providedSignature = signatureHeader.ToString();
            var providedServiceName = serviceNameHeader.ToString();

            if (string.IsNullOrWhiteSpace(providedSignature) ||
                string.IsNullOrWhiteSpace(providedServiceName))
            {
                throw new UnauthorizedException("Empty signature headers.");
            }

            var trusted = _options.TrustedServices
                .FirstOrDefault(s => string.Equals(s.Name, providedServiceName, StringComparison.OrdinalIgnoreCase));

            if (trusted is null)
            {
                throw new BadRequestException("Unknown service name.");
            }

            var validationWindow = _options.ValidationWindowMinutes;

            var now = DateTime.UtcNow;
            var validSlots = new[]
            {
                now.AddMinutes(-validationWindow).ToString("HH"),
                now.ToString("HH"),
                now.AddMinutes(validationWindow).ToString("HH")
            };

            foreach (var slot in validSlots)
            {
                if (IsValidSignature(slot, trusted.Name, trusted.Secret, providedSignature))
                    return true;
            }

            throw new UnauthorizedException("Signature mismatch.");
        }

        private bool IsValidSignature(string timeSlot, string serviceName, string secret, string providedSignature)
		{
			var payload = $"{timeSlot}{serviceName}{secret}";
			var computed = SignatureUtils.ComputeSha512(payload);
			return computed.Equals(providedSignature, StringComparison.OrdinalIgnoreCase);
		}
	}
}
