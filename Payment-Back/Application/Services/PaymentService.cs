using Payment_Back.Application.DTOs;
using Payment_Back.Application.Interfaces;
using Payment_Back.Domain.Entities;
using Payment_Back.Domain.Enums;

namespace Payment_Back.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly IPaymentProvider _provider;

        public PaymentService(IPaymentRepository repo, IPaymentProvider provider)
        {
            _repo = repo;
            _provider = provider;
        }

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
                CreatedAt = DateTime.UtcNow
            };

            await _repo.Add(payment);

            return approvalUrl;
        }

        public async Task ConfirmPayment(string externalId)
        {
            var payment = await _repo.GetByExternalId(externalId);

            if (payment == null)
                throw new Exception("Payment not found");

            payment.Status = PaymentStatus.Completed;

            await _repo.Update(payment);
        }

        public async Task FailPayment(string externalId)
        {
            var payment = await _repo.GetByExternalId(externalId);

            if (payment == null)
                throw new Exception("Payment not found");

            payment.Status = PaymentStatus.Failed;

            await _repo.Update(payment);
        }
    }
}