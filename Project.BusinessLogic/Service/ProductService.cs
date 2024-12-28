using Microsoft.Extensions.Caching.Distributed;
using Project.DataAccess.DBContext;
using Project.Models.EntityModels;
using System.Collections.Generic;
using System.Text.Json;

namespace Project.DataAccess.Services
{
    public interface IProductService : IService<Product>
    {
        Task<IEnumerable<Product>> GetProductsAsync(string search = null, int? categoryId = null, decimal? priceRange = null);
        Task<Product> GetProductByIdAsync(int productId);
    }

    public class ProductService : Service<Product>, IProductService
    {
        private readonly IDistributedCache _cache;
        public ProductService(IRepository<Product> repository,
            IDistributedCache cache) : base(repository)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string search = null, int? categoryId = null, decimal? priceRange = null)
        {
            string cacheKey = "AllProducts";
            IEnumerable<Product> products = Enumerable.Empty<Product>();

            try
            {
                string cachedProduct = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedProduct))
                {
                    products = JsonSerializer.Deserialize<IEnumerable<Product>>(cachedProduct);
                }
            }
            catch (Exception)
            {
            }

            if (products.Count() == 0)
            {
                products = await _repository.GetAllAsync();
            }

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

