using Payment_Back.Application.Interfaces;
using Payment_Back.Domain.Entities;

namespace Payment_Back.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task AddAsync(Payment payment)
        {
            // temporaire (on ajoutera EF après)
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
