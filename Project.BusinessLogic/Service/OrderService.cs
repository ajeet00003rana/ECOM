using Project.BusinessLogic.Service.BackgroundJobs;
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
        private readonly IOrderBackgroundService _orderBackgroundService;
        private readonly IProductService _productService;

        public OrderService(IRepository<Order> repository, IEmailBackgroundService emailBackgroundService, IProductService productService, IOrderBackgroundService orderBackgroundService)
        {
            _repository = repository;
            _emailBackgroundService = emailBackgroundService;
            _productService = productService;
            _orderBackgroundService = orderBackgroundService;
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            await _repository.InsertAsync(order);

            _emailBackgroundService.QueueEmail("test@example.com", "test");
            _orderBackgroundService.Execute(order);

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
