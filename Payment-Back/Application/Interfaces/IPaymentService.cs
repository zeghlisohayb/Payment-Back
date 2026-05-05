using Payment_Back.Application.DTOs;

namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePayment(CreatePaymentRequest request);

        Task ConfirmPayment(string externalId);

        Task FailPayment(string externalId);
    }
}