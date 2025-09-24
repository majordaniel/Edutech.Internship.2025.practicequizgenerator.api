using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface ILevelRepository : IRepositoryBase<Level>
    {
        Task<Level> FindLevelByCode(string code);
        Task<Level> FindLevelById(string id);
    }
}
