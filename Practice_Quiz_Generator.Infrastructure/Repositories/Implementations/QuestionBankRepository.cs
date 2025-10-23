using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class QuestionBankRepository : RepositoryBase<QuestionBank>, IQuestionBankRepository
    {
        public QuestionBankRepository(ExamPortalContext context) : base(context)
        {
        }

        public async Task<IEnumerable<QuestionBank>> FindAllQuestionByCourseId(string courseId)
        {
            return await _context.QuestionBank
               .Include(q => q.Option)
               .Include(q => q.Course)
               .Where(q => q.CourseId == courseId)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<QuestionBank>> FindAllQuestionByCourseTitle(string courseTitle)
        {
            return await _context.QuestionBank
                .Include(q => q.Option)
                .Include(q => q.Course)
                .Where(q => q.Course.Title.ToLower() == courseTitle.ToLower())
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<QuestionBank>> FindAllQuestionWithOption()
        {
            return await _context.QuestionBank
                .Include(q => q.Option)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
