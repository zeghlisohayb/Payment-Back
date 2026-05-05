namespace Payment_Back.Application.DTOs
{
    public class PaymentResponse
    {
        public string ApprovalUrl { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;
    }
}