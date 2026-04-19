using Payment_Back.Application.DTOs;
using Payment_Back.Application.Interfaces;
using Payment_Back.Application.Mappers;
using Payment_Back.Infrastructure.PaymentProviders;

namespace Payment_Back.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly PaymentProviderFactory _factory;

        public PaymentService(IPaymentRepository repo,
                              PaymentProviderFactory factory)
        {
            _repo = repo;
            _factory = factory;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var provider = _factory.Get(request.Provider);

            var payment = PaymentMapper.ToEntity(request);

            var url = await provider.CreatePayment(payment.Amount);

            await _repo.AddAsync(payment);
            await _repo.SaveChangesAsync();

            return PaymentMapper.ToResponse(payment, url);
        }
    }
}
