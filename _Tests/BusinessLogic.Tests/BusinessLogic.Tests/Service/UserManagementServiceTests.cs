using Microsoft.AspNetCore.Identity;
using Moq;
using Project.DataAccess.Services;

namespace BusinessLogic.Tests.Service
{
    public class UserManagementServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly UserManagementService _service;

        public UserManagementServiceTests()
        {
            _userManagerMock = MockUserManager();
            _service = new UserManagementService(_userManagerMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUserId_ShouldReturnUser()
        {
            // Arrange
            var user = new IdentityUser { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User1", result.UserName);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonExistingUserId_ShouldReturnNull()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync("999")).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _service.GetUserByIdAsync("999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_ShouldUpdateUser()
        {
            // Arrange
            var user = new IdentityUser { Id = "1", UserName = "OldName", Email = "old@example.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _service.UpdateUserAsync("1", "NewName", "new@example.com");

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("NewName", user.UserName);
            Assert.Equal("new@example.com", user.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_NonExistingUser_ShouldReturnFailedResult()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync("999")).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _service.UpdateUserAsync("999", "NewName", "new@example.com");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("User not found", result.Errors.First().Description);
        }

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_ShouldDeleteUser()
        {
            // Arrange
            var user = new IdentityUser { Id = "1", UserName = "User1" };
            _userManagerMock.Setup(um => um.FindByIdAsync("1")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _service.DeleteUserAsync("1");

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task DeleteUserAsync_NonExistingUser_ShouldReturnFailedResult()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync("999")).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _service.DeleteUserAsync("999");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("User not found", result.Errors.First().Description);
        }

        private static Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
