using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using Payment_Back.Application.Interfaces;

namespace Payment_Back.Infrastructure.PaymentProviders
{
    public class PaypalProvider : IPaymentProvider
    {
        private readonly PayPalClientFactory _clientFactory;

        public string Name => "PAYPAL";

        public PaypalProvider(PayPalClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<(string approvalUrl, string externalId)> CreatePayment(decimal amount, string currency)
        {
            var client = _clientFactory.Create();

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");

            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            Value = amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                        }
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = "http://localhost:4200/success",
                    CancelUrl = "http://localhost:4200/cancel"
                }
            });

            var response = await client.Execute(request);
            var result = response.Result<Order>();

            var approvalLink = result.Links.First(l => l.Rel == "approve").Href;

            return (approvalLink, result.Id);
        }
    }
}