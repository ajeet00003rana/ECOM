using Project.DataAccess.DBContext;
using Project.Models.EntityModels;

namespace Project.DataAccess.Services
{
    public interface IShoppingCartService : IService<ShoppingCart>
    {
        Task<bool> AddToCartAsync(ShoppingCart cart);
        Task<bool> RemoveFromCartAsync(int userId, int productId);
        Task<IEnumerable<ShoppingCart>> GetCartItemsAsync(int userId);
        Task<bool> ClearCartAsync(int userId);
    }

    public class ShoppingCartService : Service<ShoppingCart>, IShoppingCartService
    {
        public ShoppingCartService(IRepository<ShoppingCart> repository) : base(repository)
        {
        }

        public async Task<bool> AddToCartAsync(ShoppingCart cart)
        {
            await _repository.InsertAsync(cart);
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            var item = (await _repository.GetAllAsync())
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (item == null) return false;

            await _repository.DeleteAsync(item.CartId);
            return true;
        }

        public async Task<IEnumerable<ShoppingCart>> GetCartItemsAsync(int userId)
        {
            return (await _repository.GetAllAsync())
                .Where(c => c.UserId == userId);
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var items = (await _repository.GetAllAsync())
                .Where(c => c.UserId == userId);

            foreach (var item in items)
                await _repository.DeleteAsync(item.CartId);

            return true;
        }
    }

}

