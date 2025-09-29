namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizSetup : BaseEntity
    {
        public string QuestionType { get; set; } 
        public int NumberOfQuestions { get; set; }
        public string QuestionSource { get; set; } 
        public int Timer { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }

        public User Student { get; set; }
        public Course Course { get; set; }
    }
}
