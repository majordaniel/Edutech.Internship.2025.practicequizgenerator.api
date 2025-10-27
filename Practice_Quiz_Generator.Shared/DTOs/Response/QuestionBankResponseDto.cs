using Practice_Quiz_Generator.Shared.DTOs.Request;

namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuestionBankResponseDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string? QuestionType { get; set; }
        public string CorrectAnswer { get; set; }
        public string Source { get; set; }
        public string CourseId { get; set; }
        //public string? CourseName { get; set; }
        public List<QuestionBankOptionDto> Options { get; set; } = new();
    }
}
