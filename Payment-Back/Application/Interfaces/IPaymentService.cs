using Payment_Back.Application.DTOs;

namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);

    }
}
