using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Project.Server;
using Project.Models.ViewModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Project.Models.EntityModels;

namespace Project.Server.Tests
{
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CategoryControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddCategory_ShouldReturnCreatedAtAction_WhenCategoryIsValid()
        {
            // Arrange
            var newCategory = new CategoryViewModel
            {
                Name = "New Category",
                Description = "Category Description"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(newCategory),
                Encoding.UTF8, "application/json"
            );

            // Act
            var response = await _client.PostAsync("/api/category", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            var category = JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            Assert.Equal(newCategory.Name, category.Name);
            Assert.Equal(newCategory.Description, category.Description);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOk_WhenCategoryIsValid()
        {
            // Arrange
            var categoryToUpdate = new UpdateCategoryViewModel
            {
                CategoryId = 1,
                Name = "Updated Category",
                Description = "Updated Description"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(categoryToUpdate),
                Encoding.UTF8, "application/json"
            );

            // Act
            var response = await _client.PutAsync("/api/category", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var category = JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            Assert.Equal(categoryToUpdate.Name, category.Name);
            Assert.Equal(categoryToUpdate.Description, category.Description);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnOk_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1; // Assume category with ID 1 exists in DB

            // Act
            var response = await _client.GetAsync($"/api/category?id={categoryId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var category = JsonConvert.DeserializeObject<Category>(await response.Content.ReadAsStringAsync());
            Assert.Equal(categoryId, category.CategoryId);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 999; // Assume category with ID 999 does not exist in DB

            // Act
            var response = await _client.GetAsync($"/api/category?id={categoryId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Category not found", content);
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnOk_WhenCategoriesExist()
        {
            // Act
            var response = await _client.GetAsync("/api/category/GetAll");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var categories = JsonConvert.DeserializeObject<List<Category>>(await response.Content.ReadAsStringAsync());
            Assert.True(categories.Count > 0);
        }
    }
}
