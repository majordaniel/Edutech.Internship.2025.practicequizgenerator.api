using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class UserResponseRepository : RepositoryBase<UserResponse>, IUserResponseRepository
    {
        public UserResponseRepository(ExamPortalContext context) : base(context) { }

        public async Task<IEnumerable<UserResponse>> GetResponsesByQuizAndUserAsync(string quizId, string userId)
        {
            return await _context.UserResponses
                .Where(ur => ur.QuizId == quizId /* && ur.UserId == userId */ )  // UserId not in model; add if needed
                .ToListAsync();
        }

        public async Task CreateBulkAsync(IEnumerable<UserResponse> responses)
        {
            await _context.UserResponses.AddRangeAsync(responses);
        }

        public async Task<IEnumerable<UserResponse>> GetResponsesByQuizAndUserAsync(string quizId, string userId)
        {
            return await _context.UserResponses
                .Include(ur => ur.SelectedOption)
                .Include(ur => ur.QuizQuestion)
                    .ThenInclude(qq => qq.QuizOption)
                .Where(ur => ur.QuizId == quizId && ur.UserId == userId)
                .ToListAsync();
        }
    }
}