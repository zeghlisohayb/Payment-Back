using Payment_Back.Application.DTOs;
using Payment_Back.Application.Interfaces;
using Payment_Back.Domain.Entities;
using Payment_Back.Domain.Enums;
using Payment_Back.Infrastructure.Data;

namespace Payment_Back.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly IPaymentProvider _provider;
        private readonly PaymentDbContext _context;

        public PaymentService(
            IPaymentRepository repo,
            IPaymentProvider provider,
            PaymentDbContext context)
        {
            _repo = repo;
            _provider = provider;
            _context = context;
        }

        // CREATE PAYMENT (PayPal)

        public async Task<string> CreatePayment(CreatePaymentRequest request)
        {
            var result = await _provider.CreatePayment(request.Amount, request.Currency);

            string approvalUrl = result.approvalUrl;
            string externalId = result.externalId;

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = PaymentStatus.Pending,
                Provider = "PAYPAL",
                ExternalId = externalId,
                CreatedAt = DateTime.UtcNow,
                ItemId = request.ItemId
            };

            await _repo.Add(payment);

            return approvalUrl;
        }

        // CONFIRM PAYMENT (Webhook)

        public async Task ConfirmPayment(string externalId)
        {
            var payment = await _repo.GetByExternalId(externalId);

            if (payment == null)
                throw new Exception("Payment not found");

            // Update payment
            payment.Status = PaymentStatus.Completed;
            await _repo.Update(payment);

            // CREATE ESCROW ACCOUNT
            var escrow = new EscrowAccount
            {
                Id = Guid.NewGuid(),
                PaymentId = payment.Id,
                Balance = payment.Amount,
                Status = "HOLD",
                CreatedAt = DateTime.UtcNow
            };

            await _context.EscrowAccounts.AddAsync(escrow);

            // CREATE ESCROW TRANSACTION (HOLD)
            var escrowTransaction = new EscrowTransaction
            {
                Id = Guid.NewGuid(),
                EscrowAccountId = escrow.Id,
                Amount = payment.Amount,
                Type = "HOLD",
                CreatedAt = DateTime.UtcNow
            };

            await _context.EscrowTransactions.AddAsync(escrowTransaction);

            await _context.SaveChangesAsync();
        }

        // FAIL PAYMENT
 
        public async Task FailPayment(string externalId)
        {
            var payment = await _repo.GetByExternalId(externalId);

            if (payment == null)
                throw new Exception("Payment not found");

            payment.Status = PaymentStatus.Failed;

            await _repo.Update(payment);
        }

        // REFUND PAYMENT

        public async Task RefundPayment(string externalId)
        {
            var payment = await _repo.GetByExternalId(externalId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status != PaymentStatus.Completed)
                throw new Exception("Only completed payments can be refunded");

            var escrow = _context.EscrowAccounts
                .FirstOrDefault(e => e.PaymentId == payment.Id);

            if (escrow == null)
                throw new Exception("Escrow not found");

            // REFUND LOGIC
            escrow.Status = "REFUNDED";
            escrow.Balance = 0;

            var refundTransaction = new EscrowTransaction
            {
                Id = Guid.NewGuid(),
                EscrowAccountId = escrow.Id,
                Amount = payment.Amount,
                Type = "REFUND",
                CreatedAt = DateTime.UtcNow
            };

            await _context.EscrowTransactions.AddAsync(refundTransaction);

            payment.Status = PaymentStatus.Refunded;

            await _context.SaveChangesAsync();
        }

        // RELEASE ESCROW
        public async Task ReleaseEscrow(Guid paymentId)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status != PaymentStatus.Completed)
                throw new Exception("Payment is not completed");

            var escrow = _context.EscrowAccounts
                .FirstOrDefault(e => e.PaymentId == payment.Id);

            if (escrow == null)
                throw new Exception("Escrow not found");

            if (escrow.Status != "HOLD")
                throw new Exception("Escrow already processed");

            // RELEASE LOGIC
            escrow.Status = "RELEASED";
            escrow.Balance = 0;

            var releaseTransaction = new EscrowTransaction
            {
                Id = Guid.NewGuid(),
                EscrowAccountId = escrow.Id,
                Amount = payment.Amount,
                Type = "RELEASE",
                CreatedAt = DateTime.UtcNow
            };

            await _context.EscrowTransactions.AddAsync(releaseTransaction);

            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Payment>> GetAllPayments()
        {
            return await _repo.GetAll();
        }
    }
}