using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuizService
    {
        Task<StandardResponse<QuizResponseDto>> GenerateQuizAsync(QuizAndPersistRequestDto request);
        //Task<StandardResponse<CreateQuizResponseDto>> GenerateQuizAsync(QuizRequestDto request);
        Task<StandardResponse<QuizResponseDto>> GetQuizByIdAsync(string id);
        CreateQuizResponseDto Parse(string rawResponse);
        Task<StandardResponse<IEnumerable<QuizResponseDto>>> GetAllUserQuizzesAsync(string userId);
        Task<StandardResponse<QuizResultsResponseDto>> QuizSubmitAsync(QuizSubmissionRequestDto request);
        Task<StandardResponse<QuizResultsResponseDto>> GetQuizResultsAsync(string quizId, string userId);

        //HR
        Task<StandardResponse<QuizDetailsResponseDto>> GetQuizDetailsAsync(string quizId, string userId);
        Task<StandardResponse<QuizResultResponseDto>> SubmitQuizAsync(SubmitQuizRequestDto request, string userId);
        Task<StandardResponse<List<QuizResultResponseDto>>> GetQuizHistoryAsync(string userId);

        Task<StandardResponse<QuizUserStatsDto>> GetUserQuizStatsAsync(string userId);

    }
}
