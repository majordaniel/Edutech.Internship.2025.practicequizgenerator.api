using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizAttemptAnswer
    {
        [Key]
        public Guid Id { get; set; }
        
        // Foreign Key to QuizAttempt (string to match BaseEntity PK)
        public string QuizAttemptId { get; set; } 
        
        public Guid QuestionId { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public string? SelectedAnswerText { get; set; }
        public bool IsCorrect { get; set; }
        
        // Navigation properties
        [ForeignKey("QuizAttemptId")]
        public QuizAttempt? QuizAttempt { get; set; }
        
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }
    }
}