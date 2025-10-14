using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentCourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<StandardResponse<StudentCourseResponseDto>> GetStudentCourseByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<StandardResponse<StudentCourseResponseDto>> GetStudentCoursesAsync(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return StandardResponse<StudentCourseResponseDto>.Failed("Invalid student Id.");
            }

            var student = await _unitOfWork.UserRepository.FindUserById(studentId);
            if (student == null)
            {
                return StandardResponse<StudentCourseResponseDto>.Failed("Student not found.");
            }

            var studentCourses = await _unitOfWork.StudentCourseRepository
                .FindByCondition(sc => sc.StudentId == studentId, false)
                .Select(sc => sc.Course)
                .ToListAsync();

            if (studentCourses == null || !studentCourses.Any())
            {
                return StandardResponse<StudentCourseResponseDto>.Success(
                    "Student has no registered courses.", new StudentCourseResponseDto()
                    
                );
            }

            var response = new StudentCourseResponseDto
            {
                StudentId = studentId,
                Courses = studentCourses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Code = c.Code,
                    CreditUnit = c.CreditUnit,
                    Semester = c.Semester
                }).ToList(),
            };

            return StandardResponse<StudentCourseResponseDto>.Success("Student courses retrieved successfully.", response);
        }
    }
}
