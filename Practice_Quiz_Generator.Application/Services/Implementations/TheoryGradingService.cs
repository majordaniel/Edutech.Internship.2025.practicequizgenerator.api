using Microsoft.Extensions.Logging;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Shared.Constants;
using Practice_Quiz_Generator.Shared.CustomItems.Response;
using Practice_Quiz_Generator.Shared.DTOs.Request;
using Practice_Quiz_Generator.Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Application.Services.Implementations
{
    public class TheoryGradingService : ITheoryGradingService
    {
        private readonly IGeminiService _geminiService;
        private readonly ILogger<TheoryGradingService> _logger;

        public TheoryGradingService(IGeminiService geminiService, ILogger<TheoryGradingService> logger)
        {
            _geminiService = geminiService;
            _logger = logger;
        }

        public async Task<StandardResponse<TheoryGradingResponseDto>> GradeAsync(TheoryGradingRequestDto request)
        {
            try
            {
                var prompt = PromptTemplates.BuildTheoryGradingPrompt(request);

                var rawResponse = await _geminiService.GetLLMResponseAsync(
                       prompt
                   );

                if (string.IsNullOrEmpty(rawResponse))
                {
                    return StandardResponse<TheoryGradingResponseDto>.Failed(
                        "Failed to get response from AI grading service",
                        (int)HttpStatusCode.InternalServerError);
                }

                var gradingResult = Parse(rawResponse);

                return StandardResponse<TheoryGradingResponseDto>.Success(
                    "Theory question graded successfully", gradingResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error grading theory question");
                return StandardResponse<TheoryGradingResponseDto>.Failed($"{ex.Message}");
            }
        }

        public TheoryGradingResponseDto Parse(string rawResponse)
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

                var result = JsonSerializer.Deserialize<TheoryGradingResponseDto>(rawResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? new TheoryGradingResponseDto
                {
                    Score = 0,
                    Feedback = "Unable to evaluate answer."
                };
            }
            catch (JsonException)
            {
                return new TheoryGradingResponseDto
                {
                    Score = 0,
                    Feedback = "AI response parsing failed."
                };

            }
        }

    }
}