using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext; 
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Infrastructure.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ExamPortalContext _context;

        public QuestionRepository(ExamPortalContext context)
        {
            _context = context;
        }

        public Task<List<Question>> GetQuestionsWithAnswersByQuizIdAsync(Guid quizId)
        {
            return _context.Questions
                .Where(q => q.QuizId == quizId.ToString())
                .Include(q => q.Options)
                .ToListAsync();
        }
    }
}