using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class QuizService
{
    private readonly ExamPortalContext _context;

    public QuizService(ExamPortalContext context)
    {
        _context = context;
    }

    public async Task<QuizFormConfigurationDto> GetQuizFormConfigurationAsync(ClaimsPrincipal userPrincipal)
    {
        var userId = int.Parse(userPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var courses = await _context.Contents
            .Where(c => c.CreatedId == userId)
            .Select(c => c.Title)
            .Distinct()
            .ToListAsync();

        var config = new QuizFormConfigurationDto
        {
            Courses = courses,
            QuestionTypes = new List<string> { "MultipleChoice", "Theory" },
            MinQuestions = 5,
            MaxQuestions = 50,
            QuestionSources = new List<string> { "PastExams", "UploadedMaterials" }
        };

        return config;
    }
}