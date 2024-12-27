using Project.DataAccess.DBContext;
using Project.Models.EntityModels;

namespace Project.DataAccess.Services
{
    public interface IOrderDetailService : IService<OrderDetail>
    {
        Task<IQueryable<OrderDetail>> GetAllOrderDetails();
    }

    public class OrderDetailService : Service<OrderDetail>, IOrderDetailService
    {
        public OrderDetailService(IRepository<OrderDetail> repository) : base(repository)
        {
        }

        public Task<IQueryable<OrderDetail>> GetAllOrderDetails()
        {
            return Including();
        }
    }
}

