using Payment_Back.Application.DTOs;
using Payment_Back.Domain.Entities;

namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePayment(CreatePaymentRequest request);

        Task ConfirmPayment(string externalId);

        Task FailPayment(string externalId);

        Task RefundPayment(string externalId);

        Task ReleaseEscrow(Guid paymentId);

        Task<IEnumerable<Payment>> GetAllPayments();
    }
}