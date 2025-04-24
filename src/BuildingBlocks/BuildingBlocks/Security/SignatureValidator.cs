using BuildingBlocks.Exceptions;
using BuildingBlocks.Security.Interfaces;
using BuildingBlocks.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace BuildingBlocks.Security
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

			var expectedServiceName = _options.Service.Name;
			var expectedSecret = _options.Service.Secret;
			var validationWindow = _options.ValidationWindowMinutes;

			if (!string.Equals(providedServiceName, expectedServiceName, StringComparison.OrdinalIgnoreCase))
			{
				throw new BadRequestException("Invalid service name.");
			}

			var now = DateTime.UtcNow;
			var validSlots = new[]
			{
				now.AddMinutes(-validationWindow).ToString("HH"),
				now.ToString("HH"),
				now.AddMinutes(validationWindow).ToString("HH")
			};

			foreach (var slot in validSlots)
			{
				if (IsValidSignature(slot, providedServiceName, expectedSecret, providedSignature))
					return true;
			}

			throw new UnauthorizedException("Signature mismatch.");
		}

		private bool IsValidSignature(string timeSlot, string serviceName, string secret, string providedSignature)
		{
			var payload = $"{timeSlot}{serviceName}{secret}";
			var computed = ComputeSha512(payload);
			return computed.Equals(providedSignature, StringComparison.OrdinalIgnoreCase);
		}

		private string ComputeSha512(string input)
		{
			using var sha = SHA512.Create();
			var bytes = Encoding.UTF8.GetBytes(input);
			var hash = sha.ComputeHash(bytes);
			return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
		}
	}
}
