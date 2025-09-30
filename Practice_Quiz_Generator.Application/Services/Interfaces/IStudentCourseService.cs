using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IStudentCourseService
    {
        Task<StandardResponse<StudentCourseResponseDto>> GetStudentCourseById(string id);
        Task<StandardResponse<StudentCourseResponseDto>> GetStudentCoursesAsync(string studentId);
    }
}
