using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;

namespace Payment_Back.Infrastructure.PaymentProviders
{
    public class PayPalClientFactory
    {
        private readonly IConfiguration _configuration;

        public PayPalClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PayPalHttpClient Create()
        {
            var clientId = _configuration["PayPal:ClientId"];
            var secret = _configuration["PayPal:Secret"];

            var environment = new SandboxEnvironment(clientId, secret);

            return new PayPalHttpClient(environment);
        }
    }
}