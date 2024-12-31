using Xunit;
using Project.BusinessLogic.Service;
using System;
using System.Threading.Tasks;
using Project.Models.Api;

namespace Project.BusinessLogic.Tests
{
    public class PaymentServiceTests
    {
        private readonly IPaymentService _paymentService;

        public PaymentServiceTests()
        {
            _paymentService = new PaymentService();
        }

        [Fact]
        public async Task ProcessPaymentAsync_Should_Return_Success_When_Valid_Payment_Details()
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = "1234567812345678",
                Amount = 100m,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.TransactionId);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task ProcessPaymentAsync_Should_Return_Failure_When_CardNumber_Is_Invalid()
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = "1234",  // Invalid card number
                Amount = 100m,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid payment details.", result.ErrorMessage);
        }

        [Fact]
        public async Task ProcessPaymentAsync_Should_Return_Failure_When_Amount_Is_Zero_Or_Negative()
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = "1234567812345678",
                Amount = 0m,  // Invalid amount
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid payment details.", result.ErrorMessage);
        }

        [Fact]
        public async Task ProcessPaymentAsync_Should_Return_Failure_When_ExpiryDate_Is_In_The_Past()
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = "1234567812345678",
                Amount = 100m,
                ExpiryDate = DateTime.UtcNow.AddMonths(-1)  // Expiry date in the past
            };

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid payment details.", result.ErrorMessage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ProcessPaymentAsync_Should_Return_Failure_When_CardNumber_Is_Null_Or_Empty(string cardNumber)
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = cardNumber,
                Amount = 100m,
                ExpiryDate = DateTime.UtcNow.AddYears(1)
            };

            // Act
            var result = await _paymentService.ProcessPaymentAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid payment details.", result.ErrorMessage);
        }
    }
}
