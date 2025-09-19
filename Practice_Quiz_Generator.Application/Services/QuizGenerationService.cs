using Practice_Quiz_Generator.Domain.Models;
using System.Threading.Tasks;
using System.Linq;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext; // Or wherever your DbContext is
using Microsoft.EntityFrameworkCore;

namespace Practice_Quiz_Generator.Application.Services
{
    public class QuizGenerationService
    {
        private readonly ExamPortalContext _context;

        public QuizGenerationService(ExamPortalContext context)
        {
            _context = context;
        }

        public async Task<(bool IsValid, string Message)> ValidateRequestAsync(QuizGenerationRequestDto request, int userId)
        {
            var isCourseValid = await _context.Contents
                .AnyAsync(c => c.CourseId == request.SelectedCourseId && c.CreatedId == userId);

            if (!isCourseValid)
            {
                return (false, "Selected course is not associated with the user.");
            }

            if (request.Source == "UploadedMaterials" && !request.DocumentId.HasValue)
            {
                return (false, "DocumentId is required for 'UploadedMaterials' source.");
            }

            return (true, "Validation successful.");
        }

        public string EnqueueQuizGenerationRequest(QuizGenerationRequestDto request, int userId)
        {
            var quizRequestId = Guid.NewGuid().ToString();

            // Here, you would publish a message to a queue
            // The message would contain the request data (SelectedCourseId, NumberOfQuestions, etc.)
            // and the userId.

            // The background process would then listen to this queue,
            // pick up the request, and start the quiz generation.

            // For now, let's log the request to simulate the action.
            Console.WriteLine($"Quiz generation request {quizRequestId} enqueued for user {userId}.");

            return quizRequestId;
        }
    }
}