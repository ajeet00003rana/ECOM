using Moq;
using Project.DataAccess.DBContext;
using Project.DataAccess.Services;
using Project.Models.EntityModels;


namespace BusinessLogic.Tests.Service
{

    public class ShoppingCartServiceTests
    {
        private readonly Mock<IRepository<ShoppingCart>> _mockRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartServiceTests()
        {
            _mockRepository = new Mock<IRepository<ShoppingCart>>();
            _shoppingCartService = new ShoppingCartService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddToCartAsync_Should_Add_Item_To_Cart()
        {
            // Arrange
            var cart = new ShoppingCart { CartId = 1, UserId = 1, ProductId = 100 };
            _mockRepository.Setup(repo => repo.InsertAsync(cart)).Returns(Task.FromResult(cart));

            // Act
            var result = await _shoppingCartService.AddToCartAsync(cart);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.InsertAsync(cart), Times.Once);
        }

        [Fact]
        public async Task RemoveFromCartAsync_Should_Remove_Item_From_Cart()
        {
            // Arrange
            var userId = 1;
            var productId = 100;
            var cartItems = new List<ShoppingCart>
        {
            new ShoppingCart { CartId = 1, UserId = 1, ProductId = 100 }
        };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cartItems);
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _shoppingCartService.RemoveFromCartAsync(userId, productId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == 1)), Times.Once);
        }

        [Fact]
        public async Task RemoveFromCartAsync_Should_Return_False_If_Item_Not_Found()
        {
            // Arrange
            var userId = 1;
            var productId = 100;
            var cartItems = new List<ShoppingCart>();

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cartItems);

            // Act
            var result = await _shoppingCartService.RemoveFromCartAsync(userId, productId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetCartItemsAsync_Should_Return_Cart_Items_For_User()
        {
            // Arrange
            var userId = 1;
            var cartItems = new List<ShoppingCart>
        {
            new ShoppingCart { CartId = 1, UserId = 1, ProductId = 100 },
            new ShoppingCart { CartId = 2, UserId = 1, ProductId = 101 }
        };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cartItems);

            // Act
            var result = await _shoppingCartService.GetCartItemsAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, item => Assert.Equal(userId, item.UserId));
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task ClearCartAsync_Should_Remove_All_Items_For_User()
        {
            // Arrange
            var userId = 1;
            var cartItems = new List<ShoppingCart>
        {
            new ShoppingCart { CartId = 1, UserId = 1, ProductId = 100 },
            new ShoppingCart { CartId = 2, UserId = 1, ProductId = 101 }
        };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cartItems);
            _mockRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _shoppingCartService.ClearCartAsync(userId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == 1)), Times.Once);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == 2)), Times.Once);
        }
    }

}
