using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class DepartmentRepository : RepositoryBase<Department> , IDepartmentRepository
    {
        public DepartmentRepository(ExamPortalContext context) : base(context) { }
       
    }
}
