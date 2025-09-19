using Practice_Quiz_Generator.Domain.Models;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IQuizAttemptRepository
    {
        Task SaveAttemptAsync(QuizAttempt quizAttempt);
        Task<QuizAttempt> GetQuizAttemptByIdAsync(Guid quizAttemptId);
    }
}