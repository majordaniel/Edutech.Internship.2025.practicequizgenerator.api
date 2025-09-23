using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizValidationService : IQuizValidationService
    {
        private readonly IStudentCourseService _studentCourseService;

        public QuizValidationService(IStudentCourseService studentCourseService)
        {
            _studentCourseService = studentCourseService;
        }

        public async Task<bool> ValidateRequest(QuizGenerationRequestDto request, string studentId)
        {
            if (request.Source == "UploadedMaterials" && string.IsNullOrEmpty(request.DocumentId))
            {
                return false; // DocumentId is required for UploadedMaterials
            }
            if (request.NumberOfQuestions < 5 || request.NumberOfQuestions > 50)
            {
                return false;
            }

            var studentCourseIds = await _studentCourseService.GetStudentCourseIdsAsync(studentId);

            if (!studentCourseIds.Contains(request.SelectedCourseId))
            {
                return false;
            }

            // If we get this far, the request is valid.
            return true;
        }
    }
}