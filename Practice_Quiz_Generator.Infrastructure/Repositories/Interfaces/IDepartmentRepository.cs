using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository : IRepositoryBase<Department>
    {
        Task<Department> FindDepartmentById(string id);
        Task<Department> FindDepartmentByName(string name);
        //Task<Department> FindDepartmentsNotDeleted();
    }
}
