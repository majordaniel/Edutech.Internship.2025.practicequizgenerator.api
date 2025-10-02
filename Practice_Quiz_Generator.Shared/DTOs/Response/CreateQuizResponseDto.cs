using Microsoft.AspNetCore.Http;
using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class CreateQuizResponseDto
    {
        public string QizId { get; set; }
        public List<QuizQuestionDto> Questions { get; set; } = new();
    }
}
