using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Project.DataAccess.DBContext;
using Project.DataAccess.Services;
using Project.Models.EntityModels;
using System.Text.Json;

namespace BusinessLogic.Tests.Service
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            _mockCache = new Mock<IDistributedCache>();
            _productService = new ProductService(_mockRepository.Object, _mockCache.Object);
        }

        [Fact]
        public async Task GetProductsAsync_Should_Return_Cached_Products()
        {
            // Arrange
            var cachedProducts = new List<Product>
            {
                new Product { ProductId = 1, Name = "Cached Product 1", CategoryId = 1, Price = 100 },
                new Product { ProductId = 2, Name = "Cached Product 2", CategoryId = 2, Price = 200 }
            };

            string cachedData = JsonSerializer.Serialize(cachedProducts);

            _mockCache
                .Setup(cache => cache.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((string key, CancellationToken token) =>
                {
                    if (key == "AllProducts")
                    {
                        var byteArray = System.Text.Encoding.UTF8.GetBytes(cachedData);
                        return byteArray;
                    }
                    return null;
                });

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Cached Product 1", result.First().Name);

            _mockCache.Verify(cache => cache.GetAsync(
                It.Is<string>(k => k == "AllProducts"),
                It.IsAny<CancellationToken>()),
                Times.Once);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Never);
        }

        [Fact]
        public async Task GetProductsAsync_Should_Fallback_To_Repository_If_No_Cache()
        {
            // Arrange
            _mockCache
                .Setup(cache => cache.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null);

            var productsFromRepo = new List<Product>
            {
                new Product { ProductId = 1, Name = "Repo Product 1", CategoryId = 1, Price = 100 },
                new Product { ProductId = 2, Name = "Repo Product 2", CategoryId = 2, Price = 200 }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(productsFromRepo);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Repo Product 1", result.First().Name);

            _mockCache.Verify(cache => cache.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
                Times.Once);

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }


    }

}
