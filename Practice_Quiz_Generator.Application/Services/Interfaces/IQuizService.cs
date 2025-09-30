using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuizService
    {
        Task<StandardResponse<QuizResponseDto>> GenerateQuizAsync(QuizRequestDto request);
        QuizResponseDto Parse(string rawResponse);
    }
}
