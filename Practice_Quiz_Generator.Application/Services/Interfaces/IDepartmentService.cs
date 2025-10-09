using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<StandardResponse<DepartmentResponseDto>> DeleteDepartmnetAsync(string id);
        Task<StandardResponse<DepartmentResponseDto>> UpdateDepartmentAsync(string id, CreateDepartmentRequestDto departmentRequest);
        Task<StandardResponse<DepartmentResponseDto>> GetDepartmentByNameAsync(string name);
        Task<StandardResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(string id);
        Task<StandardResponse<IEnumerable<DepartmentResponseDto>>> GetAllDepartmentAsync();
        Task<StandardResponse<IEnumerable<DepartmentResponseDto>>> GetAllActiveDepartmentAsync();
        Task<StandardResponse<DepartmentResponseDto>> CreateDepartmentAsync(CreateDepartmentRequestDto departmentRequest);

    }
}
