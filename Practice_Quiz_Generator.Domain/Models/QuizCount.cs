namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizCount : BaseEntity
    {
        public int count { get; set; }
        public string userId { get; set; }
        public User User { get; set; }
    }
}
