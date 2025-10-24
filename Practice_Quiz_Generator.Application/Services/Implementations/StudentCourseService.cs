using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
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
            try
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
            catch (Exception ex)
            {
                return StandardResponse<StudentCourseResponseDto>.Failed($"An error occurred while retrieving student courses: {ex.Message}");
            }
        }


        public async Task<StandardResponse<string>> RegisterCourseAsync(RegisterCourseRequestDto request)
        {
            try
            {
                var course = await _unitOfWork.CourseRepository.FindCourseById(request.CourseId);
                if (course == null)
                    return StandardResponse<string>.Failed("Course not found.");

                var student = await _unitOfWork.UserRepository.FindUserById(request.StudentId);
                if (student == null)
                    return StandardResponse<string>.Failed("Student not found.");

                var alreadyRegistered = await _unitOfWork.StudentCourseRepository
                    .IsCourseAlreadyRegistered(request.StudentId, request.CourseId);
                if (alreadyRegistered)
                    return StandardResponse<string>.Failed("You have already registered for this course.");

                var registration = new StudentCourse
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    Status = "Registered"
                };

                await _unitOfWork.StudentCourseRepository.CreateAsync(registration);
                await _unitOfWork.SaveChangesAsync();

                return StandardResponse<string>.Success("Course registration successful.", registration.Id);
            }
            catch (Exception ex)
            {
                return StandardResponse<string>.Failed($"An error occurred during registration: {ex.Message}");
            }
        }
    }
}
