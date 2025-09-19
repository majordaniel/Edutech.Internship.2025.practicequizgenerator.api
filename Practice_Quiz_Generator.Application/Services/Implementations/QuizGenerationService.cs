using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizGenerationService : IQuizGenerationService
    {
        private readonly ExamPortalContext _context;
        private readonly IUserService _userService;
        
        public QuizGenerationService(ExamPortalContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<string> GenerateQuizAsync(QuizGenerationRequestDto request)
        {
            var studentId = _userService.GetCurrentUserId();

            var user = await _context.Users.FindAsync(int.Parse(studentId));
            var content = await _context.Contents.FindAsync(request.ContentId); // Assuming you add ContentId to your DTO

            var questionsQuery = _context.Questions.AsQueryable();

            if (request.Source == "UploadedMaterials" && !request.DocumentId.HasValue)
            {
                questionsQuery = questionsQuery.Where(q => q.DocumentId.ToString() == request.DocumentId.ToString());
            }
            else
            {
                questionsQuery = questionsQuery.Where(q => q.Source == request.Source);
            }

            var selectedQuestions = await questionsQuery
                .OrderBy(q => Guid.NewGuid())
                .Take(request.NumberOfQuestions)
                .ToListAsync();

            if (selectedQuestions.Count < request.NumberOfQuestions)
            {
                throw new InvalidOperationException("Not enough questions found to generate the quiz.");
            }

            var newQuiz = new Quiz
            {
                User = user,
                Content = content,

                UserId = user?.Id.ToString() ?? throw new InvalidOperationException("User not found"),
                ContentId = request.ContentId.ToString(),
                GeneratedDate = DateTime.UtcNow,
                Status = "Generated",
                Questions = selectedQuestions
            };

            _context.Quizzes.Add(newQuiz);
            await _context.SaveChangesAsync();

            return newQuiz.Id.ToString();
        }
    }
}