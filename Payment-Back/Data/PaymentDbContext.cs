using Microsoft.EntityFrameworkCore;
using Payment_Back.Domain.Entities;
using System.Collections.Generic;

namespace Payment_Back.Infrastructure.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
    }
}