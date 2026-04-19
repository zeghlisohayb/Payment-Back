using Payment_Back.Application.Interfaces;

namespace Payment_Back.Infrastructure.PaymentProviders
{
    public class PaymentProviderFactory
    {
        private readonly IEnumerable<IPaymentProvider> _providers;

        public PaymentProviderFactory(IEnumerable<IPaymentProvider> providers)
        {
            _providers = providers;
        }

        public IPaymentProvider Get(string name)
        {
            var provider = _providers.FirstOrDefault(p => p.Name == name);

            if (provider == null)
                throw new Exception("Provider not found");

            return provider;
        }
    }
}
