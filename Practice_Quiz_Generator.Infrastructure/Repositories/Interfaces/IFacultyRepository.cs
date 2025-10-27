using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IFacultyRepository : IRepositoryBase<Faculty>
    {
        Task<Faculty> FindFacultyById(string id);
        Task<Faculty> FindFacultyByName(string name);
    }
}
