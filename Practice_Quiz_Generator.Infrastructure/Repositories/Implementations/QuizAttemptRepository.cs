using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class QuizAttemptRepository : RepositoryBase<QuizAttempt>, IQuizAttemptRepository
    {
        public QuizAttemptRepository(ExamPortalContext context) : base(context) { }

        public async Task<QuizAttempt> GetAttemptByQuizAndUserAsync(string quizId, string userId)
        {
            return await _context.QuizAttempts
                .FirstOrDefaultAsync(qa => qa.QuizId == quizId && qa.UserId == userId);
        }
    }
}