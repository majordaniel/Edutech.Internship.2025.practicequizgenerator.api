using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class QuizRepository : RepositoryBase<Quiz>, IQuizRepository
    {
        public QuizRepository(ExamPortalContext context) : base(context) { }

        public async Task<Quiz> GetQuizWithQuestions(string quizId)
        {
            return await _context.Quizzes
                  .Include(q => q.QuizQuestion)
                      .ThenInclude(qq => qq.QuizOption)
                  .FirstOrDefaultAsync(q => q.Id == quizId.Trim());
        }

        public async Task<IEnumerable<Quiz>> GetAllUserQuizzesWithQuestions(string userId)
        {
            return await _context.Quizzes
                .Where(q => q.UserId == userId)
                .Include(q => q.QuizQuestion)
                    .ThenInclude(qq => qq.QuizOption)
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizWithQuestionsAsync(string quizId)
        {
            return await _context.Set<Quiz>()
                .Include(q => q.QuizQuestion)
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task<List<Quiz>> GetQuizzesByUserIdAsync(string userId)
        {
            return await _context.Set<Quiz>()
               .Where(q => q.UserId == userId)
                .Include(q => q.QuizQuestion)
                .ToListAsync();
        }

    }
}

public class QuizAttemptRepository : RepositoryBase<QuizAttempt>, IQuizAttemptRepository
{
    public QuizAttemptRepository(ExamPortalContext context) : base(context)
    {
    }

  

    public async Task<List<QuizAttempt>> GetAttemptsByUserIdAsync(string userId) 
    {
        return await _context.Set<QuizAttempt>()
            .Where(q => q.UserId == userId)
            .Include(qa => qa.Quiz)
            .OrderByDescending(qa => qa.AttemptDate)
            .ToListAsync();
    }

    public async Task<QuizAttempt?> GetAttemptWithDetailsAsync(string attemptId)
    {
        return await _context.Set<QuizAttempt>()
            .Include(qa => qa.Quiz)
                .ThenInclude(q => q.QuizQuestion)
            .FirstOrDefaultAsync(qa => qa.Id == attemptId);
    }

    public async Task<QuizAttempt> GetAttemptByQuizAndUserAsync(string quizId, string userId)
    {
        return await _context.QuizAttempts
            .FirstOrDefaultAsync(qa => qa.QuizId == quizId && qa.UserId == userId);
    }

    public Task<IQueryable<QuizAttempt>> FindByCondition(Expression<Func<QuizAttempt, bool>> expression)
        => Task.FromResult(_context.Set<QuizAttempt>().Where(expression));
}
