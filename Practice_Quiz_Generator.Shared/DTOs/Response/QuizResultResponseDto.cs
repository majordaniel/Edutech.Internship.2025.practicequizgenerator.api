namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizResultsResponseDto
    {
        public int Score { get; set; }
        public double Percentage { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeSpent { get; set; }
        public List<QuestionResultsDto> QuestionResults { get; set; } = new List<QuestionResultsDto>();
    }

    public class QuestionResultsDto
    {
        public string QuizQuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool IsCorrect { get; set; }
        public string UserAnswerText { get; set; }  // If wrong
        public string CorrectAnswerText { get; set; }  // If wrong
    }
}