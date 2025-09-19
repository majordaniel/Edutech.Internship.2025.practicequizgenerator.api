namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizResultResponseDto
    {
        public Guid QuizId { get; set; }
        public string StudentId { get; set; }
        
        // Final Score metrics
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public decimal Percentage { get; set; } 

        // Detailed results for each question
        public List<QuestionResultDto> QuestionResults { get; set; } = new List<QuestionResultDto>();
    }
}