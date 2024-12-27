using Project.DataAccess.DBContext;

namespace Project.DataAccess.Services
{
    public interface IProjectService : IService<Models.EntityModels.Project>
    {
        IQueryable<Models.EntityModels.Project> GetAllProject();
    }

    public class ProjectService : Service<Models.EntityModels.Project>, IProjectService
    {
        public ProjectService(IRepository<Models.EntityModels.Project> repository) : base(repository)
        {
        }

        public IQueryable<Models.EntityModels.Project> GetAllProject()
        {
            var projects = _repository.Including().Select(x => new Models.EntityModels.Project
            {
                Name = x.Id.ToString(),
                Id = x.Id,
                Detail = x.Detail
            });

            return projects;
        }



    }
}
