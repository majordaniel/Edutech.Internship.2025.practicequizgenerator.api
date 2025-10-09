using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<StandardResponse<UserResponseDto>> GetUserByEmailAsync(string email);
        Task<StandardResponse<UserResponseDto>> GetUserByIdAsync(string id);
        Task<StandardResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
    }
}
