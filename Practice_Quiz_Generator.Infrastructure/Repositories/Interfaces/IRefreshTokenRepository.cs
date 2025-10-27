using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        Task<RefreshToken> GetValidRefreshTokenAsync(string token, string userId);
    }
}
