using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IQuizRepository : IRepositoryBase<Quiz>
    {
        Task<Quiz> GetQuizWithQuestions(string quizId);
        Task<IEnumerable<Quiz>> GetAllUserQuizzesWithQuestions(string userId);
    }

    public interface IQuizAttemptRepository : IRepositoryBase<QuizAttempt>
    {
         Task<List<QuizAttempt>> GetAttemptsByUserIdAsync(string userId);
         Task<QuizAttempt?> GetAttemptWithDetailsAsync(string attemptId);

        
        // Task<QuizAttempt> GetAttemptsByUserIdAsync(string userId);
        // Task<IEnumerable<QuizAttempt?>> GetAttemptWithDetailsAsync(string attemptId);
    }
}
