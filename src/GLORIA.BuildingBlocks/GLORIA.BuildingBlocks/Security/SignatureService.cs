using Microsoft.Extensions.Options;

namespace GLORIA.BuildingBlocks.Security
{
    public class SignatureService : ISignatureService
    {
        private readonly SecurityValidationOptions _options;

        public SignatureService(IOptions<SecurityValidationOptions> options)
        {
            _options = options.Value;
        }

        public string Generate()
        {
            return Generate(DateTime.UtcNow);
        }

        public string Generate(DateTime utcNow)
        {
            var slot = utcNow.ToString("HH");
            var payload = $"{slot}{_options.Service.Name}{_options.Service.Secret}";
            return SignatureUtils.ComputeSha512(payload);
        }

        public string ServiceName => _options.Service.Name;  
    }
}
