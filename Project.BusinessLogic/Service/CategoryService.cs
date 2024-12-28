using Project.DataAccess.DBContext;
using Project.Models.EntityModels;

namespace Project.DataAccess.Services
{
    public interface ICategoryService : IService<Category>
    {
    }

    public class CategoryService : Service<Category>, ICategoryService
    {
        public CategoryService(IRepository<Category> repository) : base(repository)
        {
        }
    }
}

