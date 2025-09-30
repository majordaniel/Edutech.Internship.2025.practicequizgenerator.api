namespace Practice_Quiz_Generator.Domain.Models
{
    public class UserResponse
    {
        public bool IsCorrect { get; set; }
        public string QuizId { get; set; }
        public string QuestionId { get; set; }
        public string SelectedOptionId { get; set; }
        /*public int UserId { get; set; }*/
        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
        public Option SelectedOption { get; set; }
        /*        public User User { get; set; }*/
    }
}
