namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentProvider
    {
        string Name { get; }

        Task<string> CreatePayment(decimal amount);
    }
}
