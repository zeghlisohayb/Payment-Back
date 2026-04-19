namespace Payment_Back.Application.DTOs
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public string ApprovalUrl { get; set; }
        public string Status { get; set; }
    }
}
