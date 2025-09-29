using Practice_Quiz_Generator.Application.Services.Interfaces;
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

        public QuizService(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<StandardResponse<QuizResponseDto>> GenerateQuizAsync(QuizRequestDto quizRequest)
        {
            try
            {
                if (quizRequest == null)
                {
                    return StandardResponse<QuizResponseDto>.Failed("Uploaded text cannot be empty");
                }

                if (quizRequest.NumberOfQuestions <= 0)
                {
                    return StandardResponse<QuizResponseDto>.Failed("Number of questions must be greater than zero");
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
                    Options = new List<string> { "Option A", "Option B", "Option C", "Option D" },
                    CorrectOptionIndex = 0
                }
            }
                };
            }
        }
    }
}


