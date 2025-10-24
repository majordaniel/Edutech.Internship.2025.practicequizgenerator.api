using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces
{
    public interface IQuestionBankRepository : IRepositoryBase<QuestionBank>
    {
        Task<IEnumerable<QuestionBank>> FindAllQuestionWithOption();
        Task<IEnumerable<QuestionBank>> FindAllQuestionByCourseId(string courseId);
        Task<IEnumerable<QuestionBank>> FindAllQuestionByCourseTitle(string courseTitle);
        Task<IEnumerable<QuestionBank>> FindRandomQuestionsByCourseId(string courseId, int count);
    }
}
