namespace Practice_Quiz_Generator.Domain.Models
{
    public class Option
    {
        public int Id { get; set; }  // ✅ Primary Key
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public string QuizQuestionId { get; set; }
        public QuizQuestion QuizQuestion { get; set; }
    }
}