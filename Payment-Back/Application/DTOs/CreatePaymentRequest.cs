namespace Payment_Back.Application.DTOs
{
    public class CreatePaymentRequest
    {
        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "USD";

        public string ItemId { get; set; } = string.Empty;
    }
}
