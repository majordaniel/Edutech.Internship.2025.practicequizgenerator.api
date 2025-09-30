using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class QuizRepository : RepositoryBase<Quiz> , IQuizRepository
    {
        public QuizRepository(ExamPortalContext context) : base(context) { }


       
    }
}
