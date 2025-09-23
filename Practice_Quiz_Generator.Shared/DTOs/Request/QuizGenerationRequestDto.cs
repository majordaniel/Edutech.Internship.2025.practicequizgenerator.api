namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuizGenerationRequestDto
    {
        public string SelectedCourseId { get; set; }
        public string QuestionType { get; set; }
        public int NumberOfQuestions { get; set; }
        public string Source { get; set; }
        // public string? DocumentId { get; set; }
        public string? DocumentId { get; set; }

        public required int ContentId { get; set; }
    }
}