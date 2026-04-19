using Payment_Back.Application.Interfaces;

namespace Payment_Back.Infrastructure.PaymentProviders
{
    public class PaypalProvider : IPaymentProvider
    {
        public string Name => "PayPal";

        public async Task<string> CreatePayment(decimal amount)
        {
            await Task.Delay(300);
            return "https://paypal.com/approve";
        }
    }
}
