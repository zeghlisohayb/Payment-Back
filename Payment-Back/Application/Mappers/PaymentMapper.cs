using Payment_Back.Application.DTOs;
using Payment_Back.Domain.Entities;
using Payment_Back.Domain.Enums;

namespace Payment_Back.Application.Mappers
{
    public static class PaymentMapper
    {
        public static Payment ToEntity(CreatePaymentRequest request)
        {
            return new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Provider = "PAYPAL"
            };
        }

        public static PaymentResponse ToResponse(Payment payment, string approvalUrl)
        {
            return new PaymentResponse
            {
                PaymentId = payment.Id,
                ApprovalUrl = approvalUrl,
                Status = payment.Status.ToString(),
                Amount = payment.Amount,
                Currency = payment.Currency
            };
        }
    }
}