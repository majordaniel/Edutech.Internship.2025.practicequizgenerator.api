using Practice_Quiz_Generator.Shared.DTOs.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Interfaces
{
    public interface IQuizSubmissionService
    {
        Task<QuizResultResponseDto> SubmitAndGradeAsync(QuizSubmissionDto submissionDto);
    }
}
