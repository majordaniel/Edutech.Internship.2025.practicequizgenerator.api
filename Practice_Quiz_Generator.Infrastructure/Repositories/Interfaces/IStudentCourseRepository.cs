using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IStudentCourseRepository : IRepositoryBase<StudentCourse>
    {
        Task<List<Course>> GetCoursesForStudentAsync(string studentId);
    }
}
