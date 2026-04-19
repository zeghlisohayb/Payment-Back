namespace Payment_Back.Application.DTOs
{
    public class CreatePaymentRequest
    {
        public decimal Amount { get; set; }
        public string Provider { get; set; }
    }
}
