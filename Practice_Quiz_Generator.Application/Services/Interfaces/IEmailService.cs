using Practice_Quiz_Generator.Shared.CustomItems.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<StandardResponse<string>> SendEmailAsync(string to, string subject, string body);
        Task<string> GenerateEmailConfirmationLinkAsync(string email, string token, string scheme);
        Task<string> GeneratePasswordResetLinkAsync(string email, string token, string scheme);
    }
}
