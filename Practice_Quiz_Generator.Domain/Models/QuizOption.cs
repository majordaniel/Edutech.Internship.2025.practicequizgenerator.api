using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizOption : BaseEntity
    {
        public string QuizOptionText { get; set; }
        public bool IsCorrect { get; set; }
        public string QuizQuestionId { get; set; }

        public QuizQuestion QuizQuestion { get; set; }
    }
}
