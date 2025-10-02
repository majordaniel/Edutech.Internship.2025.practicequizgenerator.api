using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class CourseRepository : RepositoryBase<Course> , ICourseRepository
    {
        public CourseRepository(ExamPortalContext context) : base(context) { }


        public async Task<Course> FindCourseById(string id)
        {
            return await FindByCondition(c => c.Id == id, false)
                .FirstOrDefaultAsync();
        }

        public async Task<Course> FindCourseByName(string name)
        {
            return await FindByCondition(c => c.Title.ToLower() == name.ToLower(), false)
                 .FirstOrDefaultAsync();
        }


    }
}
