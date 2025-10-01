using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ExamPortalContext context) : base(context)
        {
        }
        public async Task<RefreshToken> GetValidRefreshTokenAsync(string token, string userId)
        {
            return await FindByCondition(r =>
                r.Token == token &&
                r.UserId == userId &&
                r.ExpiryDate > DateTime.UtcNow &&
                !r.IsUsed, false).FirstOrDefaultAsync();
        }
    }
}
