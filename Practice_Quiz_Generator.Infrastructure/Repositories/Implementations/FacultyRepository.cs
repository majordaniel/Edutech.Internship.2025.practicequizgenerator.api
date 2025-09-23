using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class FacultyRepository : RepositoryBase<Faculty> , IFacultyRepository
    {
        public FacultyRepository(ExamPortalContext context) : base(context) { }


        public async Task<Faculty> FindFacultyById(string id)
        {
            return await FindByCondition(f => f.Id == id, false).FirstOrDefaultAsync();
        }

        public async Task<Faculty> FindFacultyByName(string name)
        {
            return await FindByCondition(f => f.Name.ToLower() == name.ToLower(), false)
                 .FirstOrDefaultAsync();
        }

    }
}
