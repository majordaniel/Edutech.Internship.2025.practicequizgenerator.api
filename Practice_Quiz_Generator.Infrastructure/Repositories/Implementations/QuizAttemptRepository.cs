using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class QuizAttemptRepository : IQuizAttemptRepository
    {
        private readonly ExamPortalContext _context;

        public QuizAttemptRepository(ExamPortalContext context)
        {
            _context = context;
        }

        public async Task SaveAttemptAsync(QuizAttempt quizAttempt)
        {
            _context.QuizAttempts.Add(quizAttempt);
            await _context.SaveChangesAsync();
        }

        public async Task<QuizAttempt> GetQuizAttemptByIdAsync(Guid quizAttemptId)
        {
            return await _context.QuizAttempts.FindAsync(quizAttemptId);
        }
    }
}
