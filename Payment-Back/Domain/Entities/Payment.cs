using Payment_Back.Domain.Enums;

namespace Payment_Back.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public PaymentStatus Status { get; set; }

        public string Provider { get; set; } = "PAYPAL";

        public string ExternalId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string ItemId { get; set; } = string.Empty;
    }
}

