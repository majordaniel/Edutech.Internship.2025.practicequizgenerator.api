namespace Practice_Quiz_Generator.Domain.Models
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; }
        public bool? IsCompleted { get; set; }
        public int? TimeSpent { get; set; }
        //public string? SouceText { get; set; }
        public string QuestionType { get; set; }
        public int NumberOfQuestions { get; set; }
        public string QuestionSource { get; set; } //File upload
        public int Timer { get; set; }
        public string CourseId { get; set; }
        //public string QuizSetupId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
        //public QuizSetup QuizSetup { get; set; }
        public ICollection<QuizQuestion> QuizQuestion { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
        public ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
    }
}