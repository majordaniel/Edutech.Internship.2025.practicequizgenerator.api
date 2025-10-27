using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class LevelRepository : RepositoryBase<Level> , ILevelRepository
    {
       public LevelRepository(ExamPortalContext contect) : base(contect) { }

        public async Task<Level> FindLevelById(string id)
        {
            return await FindByCondition(l => l.Id == id, false)
                .FirstOrDefaultAsync();
        }

        public async Task<Level> FindLevelByCode(string code)
        {
            return await FindByCondition(l => l.Code == code, false)
                .FirstOrDefaultAsync();
        }

    }
}
