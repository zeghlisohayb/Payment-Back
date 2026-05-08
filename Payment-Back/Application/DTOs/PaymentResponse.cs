namespace Payment_Back.Application.DTOs
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }

        public string ApprovalUrl { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}