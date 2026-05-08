namespace Payment_Back.Domain.Entities
{
    public class EscrowTransaction
    {
        public Guid Id { get; set; }

        public Guid EscrowAccountId { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; } = string.Empty; // HOLD / RELEASE

        public DateTime CreatedAt { get; set; }
    }
}