namespace Practice_Quiz_Generator.Domain.Models
{
    public class UserResponse : BaseEntity
    {
        public bool IsCorrect { get; set; }
        public string QuizId { get; set; }
        public string QuizQuestionId { get; set; }
        public string SelectedOptionId { get; set; }
        public string UserId { get; set; }
        public Quiz Quiz { get; set; }
        public QuizQuestion QuizQuestion { get; set; }
        public QuestionBank Question { get; set; }
        public QuizOption SelectedOption { get; set; }
        public User User { get; set; }
    }
}
