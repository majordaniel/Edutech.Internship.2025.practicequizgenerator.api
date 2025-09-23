using Practice_Quiz_Generator.Shared.DTOs.Request;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuizValidationService
    {
        Task<bool> ValidateRequest(QuizGenerationRequestDto request, string studentId);
    }
}