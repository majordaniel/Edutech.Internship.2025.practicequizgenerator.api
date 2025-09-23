using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class LevelRepository : RepositoryBase<Level> , ILevelRepository
    {
       public LevelRepository(ExamPortalContext contect) : base(contect) { }


    }
}
