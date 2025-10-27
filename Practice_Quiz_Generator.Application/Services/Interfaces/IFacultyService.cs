using Azure;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<StandardResponse<Faculty>> DeleteFacultyAsync(string id);
        Task<StandardResponse<FacultyResponseDto>> UpdateFacultyAsync(string id, FacultyRequestDto facultyRequest);
        Task<StandardResponse<FacultyResponseDto>> GetFacultyByNameAsync(string name);
        Task<StandardResponse<IEnumerable<FacultyResponseDto>>> GetAllFacultiesAsync();
        Task<StandardResponse<FacultyResponseDto>> GetFacultyByIdAsync(string id);
        Task<StandardResponse<FacultyResponseDto>> CreateFacultyAsync(FacultyRequestDto facultyRequest);
    }
}
