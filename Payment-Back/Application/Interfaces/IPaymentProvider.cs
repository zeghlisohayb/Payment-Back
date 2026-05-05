namespace Payment_Back.Application.Interfaces
{
    public interface IPaymentProvider
    {
        string Name { get; }

        Task<(string approvalUrl, string externalId)> CreatePayment(decimal amount, string currency);
    }
}