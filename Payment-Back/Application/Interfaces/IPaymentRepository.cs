using Payment_Back.Domain.Entities;

namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task Add(Payment payment);

        Task<Payment> GetByExternalId(string externalId);

        Task Update(Payment payment);
    }
}