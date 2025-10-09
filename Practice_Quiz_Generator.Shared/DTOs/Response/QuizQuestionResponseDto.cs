namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizQuestionResponseDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<QuizOptionResponseDto> Options { get; set; } = new();
        public int CorrectOptionIndex { get; set; }
    }
}
