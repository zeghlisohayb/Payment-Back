using Microsoft.EntityFrameworkCore;
using Payment_Back.Application.Interfaces;
using Payment_Back.Domain.Entities;
using Payment_Back.Infrastructure.Data;

namespace Payment_Back.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task Add(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> GetByExternalId(string externalId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.ExternalId == externalId);
        }

        public async Task Update(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> GetAll()
        {
            return await _context.Payments.ToListAsync();
        }
    }
}