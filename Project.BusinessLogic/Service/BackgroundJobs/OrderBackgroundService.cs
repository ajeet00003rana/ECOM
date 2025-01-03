using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.DataAccess.Services;
using Project.Models.EntityModels;
using System.Collections.Concurrent;

namespace Project.BusinessLogic.Service.BackgroundJobs
{
    public interface IOrderBackgroundService
    {
        void Execute(Order order);
    }

    public class OrderBackgroundService : BackgroundService, IOrderBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentQueue<Order> _queue = new ConcurrentQueue<Order>();

        public OrderBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Execute(Order order)
        {
            _queue.Enqueue((order));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var orderData))
                {
                    using var scope = _serviceProvider.CreateScope();
                    var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                    foreach (var item in orderData.OrderDetails)
                    {
                        var product = await productService.GetByIdAsync(item.ProductId);
                        product.StockCount -= item.Quantity;
                        await productService.UpdateAsync(product);
                    }
                }
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
