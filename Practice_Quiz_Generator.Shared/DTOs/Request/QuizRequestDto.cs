namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuizRequestDto
    {
        public string? QuestionType { get; set; }
        public int NumberOfQuestions { get; set; } 
        public string UploadedText { get; set; }
    }
}
