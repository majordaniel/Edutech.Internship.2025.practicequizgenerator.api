namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuizQuestionRequestDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int CorrectOptionIndex { get; set; }
    }
}
