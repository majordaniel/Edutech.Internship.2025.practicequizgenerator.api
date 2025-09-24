using Microsoft.AspNetCore.Identity;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<StandardResponse<UserResponseDto>> RegisterAsync(CreateUserRequestDto userRequest);
        Task<StandardResponse<ConfirmEmailResponseDto>> ConfirmEmailAsync(string email, string token);
        Task<StandardResponse<string>> ForgotPasswordAsync(string email);
        Task<StandardResponse<string>> ResetPasswordAsync(ResetPasswordRequestDto request);
        
    }
}
