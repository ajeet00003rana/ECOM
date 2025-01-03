using Microsoft.Extensions.DependencyInjection;
using Moq;
using Project.BusinessLogic.Service.BackgroundJobs;
using Project.DataAccess.Services;
using Project.Models.EntityModels;

namespace BusinessLogic.Tests.Service.BackgroundJobs
{
    public class OrderBackgroundServiceTests
    {
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IProductService> _mockProductService;
        private readonly OrderBackgroundService _orderBackgroundService;

        public OrderBackgroundServiceTests()
        {
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockProductService = new Mock<IProductService>();

            var serviceScope = new Mock<IServiceScope>();
            var serviceScopeFactory = new Mock<IServiceScopeFactory>();

            serviceScope.Setup(x => x.ServiceProvider).Returns(_mockServiceProvider.Object);
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
            _mockServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                                 .Returns(serviceScopeFactory.Object);
            _mockServiceProvider.Setup(x => x.GetService(typeof(IProductService)))
                                 .Returns(_mockProductService.Object);

            _orderBackgroundService = new OrderBackgroundService(_mockServiceProvider.Object);
        }

        [Fact]
        public async Task Execute_Should_Process_Queued_Orders()
        {
            // Arrange
            var productId = 1;
            var initialStock = 10;
            var quantityOrdered = 2;

            var product = new Product { ProductId = productId, StockCount = initialStock };
            var order = new Order
            {
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = productId, Quantity = quantityOrdered }
                }
            };

            _mockProductService.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockProductService.Setup(x => x.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            _orderBackgroundService.Execute(order);

            // Start the background service in a separate task
            var cts = new CancellationTokenSource();
            var executeTask = _orderBackgroundService.StartAsync(cts.Token);

            // Allow some time for the order to be processed
            await Task.Delay(1500);

            // Stop the background service
            cts.Cancel();
            await executeTask;

            // Assert
            _mockProductService.Verify(x => x.GetByIdAsync(productId), Times.Once);
            _mockProductService.Verify(x => x.UpdateAsync(It.Is<Product>(p => p.StockCount == initialStock - quantityOrdered)), Times.Once);
        }
    }
}
