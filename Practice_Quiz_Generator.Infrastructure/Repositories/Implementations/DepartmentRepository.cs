using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class DepartmentRepository : RepositoryBase<Department> , IDepartmentRepository
    {
        public DepartmentRepository(ExamPortalContext context) : base(context) { }

        public async Task<Department> FindDepartmentById(string id)
        {
            return await FindByCondition(d => d.Id == id, false).FirstOrDefaultAsync();
        }

        public async Task<Department> FindDepartmentByName(string name)
        {
            return await FindByCondition(d => d.Name.ToLower() == name.ToLower(), false)
                .FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<Department>> FindDepartmentsNotDeleted()
        //{
        //    return await FindByCondition(d => d.IsDeleted == false, false).ToListAsync();
        //}
    }
}
