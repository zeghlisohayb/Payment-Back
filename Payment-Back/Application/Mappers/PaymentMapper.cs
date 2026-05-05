/*using Payment_Back.Application.DTOs;
using Payment_Back.Domain.Entities;

namespace Payment_Back.Application.Mappers
{
    public class PaymentMapper
    {
        public static Payment ToEntity(CreatePaymentRequest request)
        {
            return new Payment
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Status = "Pending",
                Provider = request.Provider,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static PaymentResponse ToResponse(Payment payment, string url)
        {
            return new PaymentResponse
            {
                Id = payment.Id,
                ApprovalUrl = url,
                Status = payment.Status
            };
        }
    }
}*/
