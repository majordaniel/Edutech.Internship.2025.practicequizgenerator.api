using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IQuizRepository : IRepositoryBase<Quiz>
    {
        Task<Quiz?> GetQuizWithQuestions(string quizId);
    }
}
