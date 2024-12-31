using Moq;
using Project.DataAccess.DBContext;
using Project.DataAccess.Services;
using Project.Models.EntityModels;


namespace BusinessLogic.Tests.Service
{
    public class PaymentLogServiceTests
    {
        private readonly Mock<IRepository<PaymentLog>> _mockRepository;
        private readonly PaymentLogService _paymentLogService;

        public PaymentLogServiceTests()
        {
            _mockRepository = new Mock<IRepository<PaymentLog>>();
            _paymentLogService = new PaymentLogService(_mockRepository.Object);
        }

        [Fact]
        public void GetAllPaymentLogs_ShouldReturnAllLogs()
        {
            // Arrange
            var paymentLogs = new List<PaymentLog>
            {
                new PaymentLog { },
                new PaymentLog { }
            }.AsQueryable();

            _mockRepository.Setup(repo => repo.Including()).Returns(paymentLogs);

            // Act
            var result = _paymentLogService.GetAllPaymentLogs();

            // Assert
            Assert.Equal(paymentLogs.Count(), result.Count());
            // Additional assertions can be made based on properties
        }
    }

}
