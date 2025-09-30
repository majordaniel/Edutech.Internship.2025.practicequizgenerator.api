namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizQuestion : BaseEntity
    {
        public string QuestionText { get; set; }
        public int CorrectOptionIndex { get; set; }
        public string QuizId { get; set; }

        public Quiz Quiz { get; set; }
        public ICollection<QuizOption> QuizOption { get; set; }
        //public ICollection<UserResponse> UserResponses { get; set; }
    }
}

