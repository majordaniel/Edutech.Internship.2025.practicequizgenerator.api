namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuizQuestionDto
    {
        public string Question { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectOptionIndex { get; set; }
    }
}
