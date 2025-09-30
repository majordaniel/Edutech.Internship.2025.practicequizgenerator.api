using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Practice_Quiz_Generator.Shared.Constants;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System.Text.Json;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IGeminiService _geminiService;
        private readonly IUnitOfWork _unitOfWork;

        public QuizService(IGeminiService geminiService, IUnitOfWork unitOfWork)
        {
            _geminiService = geminiService;
            _unitOfWork = unitOfWork;
        }

        public async Task<StandardResponse<QuizResponseDto>> GenerateQuizAsync(QuizRequestDto quizRequest)
        {//Reminder -->> Separate concerns base on question type and source
            try
            {
                if (quizRequest == null || string.IsNullOrWhiteSpace(quizRequest.UploadedText))
                {
                    return StandardResponse<QuizResponseDto>.Failed("Uploaded text cannot be empty");
                }

                if (quizRequest.NumberOfQuestions <= 5)
                {
                    return StandardResponse<QuizResponseDto>.Failed("Number of questions must be greater than 5");
                }
                var prompt = PromptTemplates.BuildQuizPrompt(
                quizRequest.UploadedText,
                quizRequest.NumberOfQuestions
            );

                var rawResponse = await _geminiService.GetLLMResponseAsync(
                    prompt
                );

                var quizResponse = Parse(rawResponse);

                if (quizResponse.Questions == null || !quizResponse.Questions.Any())
                {
                    return StandardResponse<QuizResponseDto>.Failed("Failed to generate questions from AI");
                }

                var course = await _unitOfWork.CourseRepository.FindCourseById(quizRequest.CourseId);
                var user = await _unitOfWork.UserRepository.FindUserById(quizRequest.UserId);

                var quiz = new Quiz
                {
                    Title = course.Title,
                    QuestionType = quizRequest.QuestionType,
                    QuestionSource = quizRequest.QuestionSource,
                    Timer = quizRequest.Timer, // Reminder -->> Set to minutes
                    CourseId = quizRequest.CourseId,
                    UserId = quizRequest.UserId,
                    QuizQuestion = quizResponse.Questions.Select(q => new QuizQuestion
                    {
                        QuestionText = q.Question,
                        QuizOption = q.Options.Select((opt, index) => new QuizOption
                        {
                            QuizOptionText = opt,
                            IsCorrect = index == q.CorrectOptionIndex
                        }).ToList()
                    }).ToList()
                };

                await _unitOfWork.QuizRepository.CreateAsync(quiz);
                await _unitOfWork.SaveChangesAsync();   

                return StandardResponse<QuizResponseDto>.Success("Quiz generated successfully",
                    quizResponse

                );
            }
            catch (Exception ex)
            {
                return StandardResponse<QuizResponseDto>.Failed($"{ex.Message}");
            }
        }

        public QuizResponseDto Parse(string rawResponse)
        {
            try
            {
                rawResponse = rawResponse.Trim();
                if (rawResponse.StartsWith("```"))
                {
                    var firstLineEnd = rawResponse.IndexOf('\n');
                    rawResponse = rawResponse.Substring(firstLineEnd + 1);
                    rawResponse = rawResponse.Trim('`', '\n', '\r');
                }

                var quizResponse = JsonSerializer.Deserialize<QuizResponseDto>(rawResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return quizResponse ?? new QuizResponseDto { Questions = new List<QuizQuestionDto>() };
            }
            catch (JsonException)
            {
                return new QuizResponseDto
                {
                    Questions = new List<QuizQuestionDto>
            {
                new QuizQuestionDto
                {
                    Question = "Failed to parse AI response",
                    Options = new List<string> { "QuizOption A", "QuizOption B", "QuizOption C", "QuizOption D" },
                    CorrectOptionIndex = 0
                }
            }
                };
            }
        }
    }
}


