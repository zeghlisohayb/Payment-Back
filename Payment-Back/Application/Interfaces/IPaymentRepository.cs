using Payment_Back.Domain.Entities;

namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task SaveChangesAsync();
    }
}
