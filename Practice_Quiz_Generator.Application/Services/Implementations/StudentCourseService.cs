using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly ExamPortalContext _context;

        public StudentCourseService(ExamPortalContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetStudentCourseIdsAsync(string studentId)
        {
            var courseIds = await _context.StudentCourses
                                            .Where(sc => sc.StudentId == studentId)
                                            .Select(sc => sc.CourseId)
                                            .ToListAsync();

            return courseIds;
        }
    }
}