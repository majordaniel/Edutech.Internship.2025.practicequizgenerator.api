using Practice_Quiz_Generator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestionsWithAnswersByQuizIdAsync(Guid quizId);
    }
}
