using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    [NotMapped]
    public class Option
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public string QuizQuestionId { get; set; }

        public QuizQuestion QuizQuestion { get; set; }
    }
}
