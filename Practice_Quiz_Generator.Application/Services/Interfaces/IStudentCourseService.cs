using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IStudentCourseService
    {
        Task<StandardResponse<StudentCourseResponseDto>> GetStudentCourseByIdAsync(string id);
        Task<StandardResponse<StudentCourseResponseDto>> GetStudentCoursesAsync(string studentId);
        Task<StandardResponse<string>> RegisterCourseAsync(RegisterCourseRequestDto request);
    }
}
