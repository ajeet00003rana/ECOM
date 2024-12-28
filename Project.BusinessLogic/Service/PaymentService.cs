using Project.Models.Api;

namespace Project.BusinessLogic.Service
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    }

    public class PaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            await Task.Delay(100);

            if (string.IsNullOrWhiteSpace(request.CardNumber) ||
                request.CardNumber.Length != 16 ||
                request.Amount <= 0 ||
                request.ExpiryDate < DateTime.UtcNow)
            {
                return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid payment details."
                };
            }

            return new PaymentResult
            {
                IsSuccess = true,
                TransactionId = Guid.NewGuid().ToString()
            };
        }
    }

}
