using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<StandardResponse<IEnumerable<CourseDto>>> GetAllCoursesAsync();
    }
}
