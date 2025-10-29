using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
         Task<IQueryable<QuizAttempt>> FindByCondition(Expression<Func<QuizAttempt, bool>> expression);
         Task<QuizAttempt> GetAttemptByQuizAndUserAsync(string quizId, string userId);

        
        // Task<QuizAttempt> GetAttemptsByUserIdAsync(string userId);
        // Task<IEnumerable<QuizAttempt?>> GetAttemptWithDetailsAsync(string attemptId);
    }
}
