namespace Payment_Back.Domain.Entities
{
    public class EscrowAccount
    {
        public Guid Id { get; set; }

        public Guid PaymentId { get; set; }

        public decimal Balance { get; set; }

        public string Status { get; set; } = string.Empty; // HOLD / RELEASED

        public DateTime CreatedAt { get; set; }
    }
}