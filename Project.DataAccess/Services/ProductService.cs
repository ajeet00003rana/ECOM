using Project.DataAccess.DBContext;
using Project.Models.EntityModels;

namespace Project.DataAccess.Services
{
    public interface IProductService : IService<Product>
    {
        Task<IEnumerable<Product>> GetProductsAsync(string search = null, int? categoryId = null, decimal? priceRange = null);
        Task<Product> GetProductByIdAsync(int productId);
    }

    public class ProductService : Service<Product>, IProductService
    {
        public ProductService(IRepository<Product> repository) : base(repository)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string search = null, int? categoryId = null, decimal? priceRange = null)
        {
            var products = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId);

            if (priceRange.HasValue)
                products = products.Where(p => p.Price <= priceRange);

            return products;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await GetByIdAsync(productId);
        }
    }

}

