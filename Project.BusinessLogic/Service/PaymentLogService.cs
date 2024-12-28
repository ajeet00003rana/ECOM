using Project.DataAccess.DBContext;
using Project.Models.EntityModels;

namespace Project.DataAccess.Services
{
    public interface IPaymentLogService : IService<PaymentLog>
    {
        IQueryable<PaymentLog> GetAllPaymentLogs();
    }

    public class PaymentLogService : Service<PaymentLog>, IPaymentLogService
    {
        public PaymentLogService(IRepository<PaymentLog> repository) : base(repository)
        {
        }

        public IQueryable<PaymentLog> GetAllPaymentLogs()
        {
            return _repository.Including();
        }
    }
}
