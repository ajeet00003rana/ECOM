using Moq;
using Project.BusinessLogic.Email;
using Project.DataAccess.DBContext;
using Project.DataAccess.Services;
using Project.Models.EntityModels;

namespace BusinessLogic.Tests.Service
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepository<Order>> _mockRepository;
        private readonly Mock<IEmailBackgroundService> _mockEmailService;
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            _mockRepository = new Mock<IRepository<Order>>();
            _mockEmailService = new Mock<IEmailBackgroundService>();
            _orderService = new OrderService(_mockRepository.Object, _mockEmailService.Object);
        }

        [Fact]
        public async Task PlaceOrderAsync_ShouldInsertOrderAndQueueEmail()
        {
            // Arrange
            var order = new Order { };

            // Act
            var result = await _orderService.PlaceOrderAsync(order);

            // Assert
            _mockRepository.Verify(repo => repo.InsertAsync(order), Times.Once);
            _mockEmailService.Verify(email => email.QueueEmail("test@example.com", "test"), Times.Once);
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task GetUserOrdersAsync_ShouldReturnOrdersForUser()
        {
            // Arrange
            int userId = 1;
            var orders = new List<Order>
        {
            new Order { UserId = userId,},
            new Order { UserId = userId,}
        }.AsQueryable();

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetUserOrdersAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, order => Assert.Equal(userId, order.UserId));
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder()
        {
            // Arrange
            int orderId = 1;
            var order = new Order { OrderId = orderId, /* Initialize other properties */ };

            _mockRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ShouldUpdateStatus()
        {
            // Arrange
            int orderId = 1;
            string newStatus = "Shipped";
            var order = new Order { OrderId = orderId, Status = "Pending" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);

            // Assert
            Assert.True(result);
            Assert.Equal(newStatus, order.Status);
            _mockRepository.Verify(repo => repo.UpdateAsync(order), Times.Once);
        }
    }

}
