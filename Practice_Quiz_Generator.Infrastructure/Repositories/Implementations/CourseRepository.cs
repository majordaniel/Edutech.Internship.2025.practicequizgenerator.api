using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class CourseRepository : RepositoryBase<Course> , ICourseRepository
    {
        public CourseRepository(ExamPortalContext context) : base(context) { }
    }
}
