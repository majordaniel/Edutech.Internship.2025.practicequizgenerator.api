namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizResponseDto
    {
        public string QuizId { get; set; }
        public string Title { get; set; }
        public bool? IsCompleted { get; set; }
        public int? TimeSpent { get; set; }
        public string QuestionType { get; set; }
        public int NumberOfQuestions { get; set; }
        public string QuestionSource { get; set; } 
        public int Timer { get; set; }
        public string CourseId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalNumberOfQuiz { get; set; }
        public ICollection<QuizQuestionResponseDto> Questions { get; set; } = new List<QuizQuestionResponseDto>();
    }
}
