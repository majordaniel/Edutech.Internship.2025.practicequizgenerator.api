using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IUserResponseRepository : IRepositoryBase<UserResponse>
    {
        // Task<IEnumerable<UserResponse>> GetResponsesByQuizAndUserAsync(string quizId, string userId);
        Task CreateBulkAsync(IEnumerable<UserResponse> responses);
        Task<IEnumerable<UserResponse>> GetResponsesByQuizAndUserAsync(string quizId, string userId);
    }
}