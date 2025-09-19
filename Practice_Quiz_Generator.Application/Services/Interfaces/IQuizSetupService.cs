using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuizSetupService
    {
        Task<QuizSetupResponseDto> GetQuizSetupDataAsync(string studentId);
    }
}