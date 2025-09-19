namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuestionResultDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool IsCorrect { get; set; }
        public string StudentAnswerText { get; set; } 
        public string CorrectAnswerText { get; set; }
    }
}