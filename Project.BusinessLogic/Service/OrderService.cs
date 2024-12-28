using Project.BusinessLogic.Email;
using Project.DataAccess.DBContext;
using Project.Models.EntityModels;

namespace Project.DataAccess.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }

    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _repository;
        private readonly IEmailBackgroundService _emailBackgroundService;

        public OrderService(IRepository<Order> repository, IEmailBackgroundService emailBackgroundService)
        {
            _repository = repository;
            _emailBackgroundService = emailBackgroundService;
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            await _repository.InsertAsync(order);

            _emailBackgroundService.QueueEmail("test@example.com", "test");
            // Deduct stock for each product in OrderDetails
            //foreach (var item in order.OrderDetails)
            //{
            //    var product = await _repository.GetByIdAsync(item.ProductId);
            //    if (product == null) throw new KeyNotFoundException($"Product {item.ProductId} not found.");

            //    product.StockCount -= item.Quantity;
            //    await _repository.UpdateAsync(product);
            //}

            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return (await _repository.GetAllAsync())
                .Where(o => o.UserId == userId);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _repository.GetByIdAsync(orderId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _repository.UpdateAsync(order);
            return true;
        }
    }

}
