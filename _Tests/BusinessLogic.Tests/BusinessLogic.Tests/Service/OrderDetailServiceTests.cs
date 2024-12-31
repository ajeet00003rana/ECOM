using Moq;
using Project.DataAccess.DBContext;
using Project.DataAccess.Services;
using Project.Models.EntityModels;

namespace BusinessLogic.Tests.Service
{
    public class OrderDetailServiceTests
    {
        private readonly Mock<IRepository<OrderDetail>> _mockRepository;
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailServiceTests()
        {
            _mockRepository = new Mock<IRepository<OrderDetail>>();
            _orderDetailService = new OrderDetailService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllOrderDetails_ShouldReturnAllOrderDetails()
        {
            // Arrange
            var orderDetails = new List<OrderDetail>
            {
                new OrderDetail {  },
                new OrderDetail {  }
            }.AsQueryable();

            _mockRepository.Setup(repo => repo.Including()).Returns(orderDetails);

            // Act
            var result = await _orderDetailService.GetAllOrderDetails();

            // Assert
            Assert.Equal(orderDetails.Count(), result.Count());
        }

        [Fact]
        public async Task GetAllOrderDetails_ShouldReturnEmpty_WhenNoOrderDetails()
        {
            // Arrange
            var orderDetails = new List<OrderDetail>().AsQueryable();
            _mockRepository.Setup(repo => repo.Including()).Returns(orderDetails);

            // Act
            var result = await _orderDetailService.GetAllOrderDetails();

            // Assert
            Assert.Empty(result);
        }
    }

}
