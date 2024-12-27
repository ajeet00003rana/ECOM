namespace Project.DataAccess.DBContext
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(object id);
    }

    public class Service<T> : IService<T> where T : class
    {
        public readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IQueryable<T>> Including()
        {
            return _repository.Including();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            await _repository.InsertAsync(entity);
            return entity; 
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
            return entity; 
        }


        public async Task DeleteAsync(object id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
