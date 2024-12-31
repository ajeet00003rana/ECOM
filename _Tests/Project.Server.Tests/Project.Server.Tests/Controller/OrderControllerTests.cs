using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.DataAccess.Services;
using Project.Models.EntityModels;
using Project.Server.Controllers;

namespace Project.Server.Tests.Controller
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
        }

        [Fact]
        public async Task PlaceOrder_ShouldReturnCreatedAtAction_WhenOrderIsPlaced()
        {
            // Arrange
            var order = new Order { OrderId = 1, UserId = 2, Status = "Pending" };
            var newOrder = new Order { OrderId = 1, UserId = 2, Status = "Confirmed" };

            _mockOrderService.Setup(s => s.PlaceOrderAsync(order)).ReturnsAsync(newOrder);

            // Act
            var result = await _controller.PlaceOrder(order);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult!.Value.Should().BeEquivalentTo(newOrder);
            createdAtActionResult.RouteValues!["id"].Should().Be(newOrder.OrderId);

            _mockOrderService.Verify(s => s.PlaceOrderAsync(order), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { OrderId = orderId, UserId = 2, Status = "Pending" };

            _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetById(orderId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(order);

            _mockOrderService.Verify(s => s.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;

            _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetById(orderId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult!.Value.Should().Be("Order not found.");

            _mockOrderService.Verify(s => s.GetOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetUserOrders_ShouldReturnOk_WithUserOrders()
        {
            // Arrange
            var userId = 2;
            var orders = new List<Order>
            {
                new Order { OrderId = 1, UserId = userId, Status = "Pending" }
            };

            _mockOrderService.Setup(s => s.GetUserOrdersAsync(userId)).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetUserOrders(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(orders);

            _mockOrderService.Verify(s => s.GetUserOrdersAsync(userId), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_ShouldReturnOk_WhenOrderStatusIsUpdated()
        {
            // Arrange
            var orderId = 1;
            var status = "Shipped";

            _mockOrderService.Setup(s => s.UpdateOrderStatusAsync(orderId, status)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateOrderStatus(orderId, status);

            // Assert
            result.Should().BeOfType<OkResult>();

            _mockOrderService.Verify(s => s.UpdateOrderStatusAsync(orderId, status), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderStatus_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 1;
            var status = "Cancelled";

            _mockOrderService.Setup(s => s.UpdateOrderStatusAsync(orderId, status)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateOrderStatus(orderId, status);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult!.Value.Should().Be("Order not found.");

            _mockOrderService.Verify(s => s.UpdateOrderStatusAsync(orderId, status), Times.Once);
        }
    }
}
