using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public required int QuizId { get; set; } // FK to Quiz

        [Required]
        public required string QuestionText { get; set; }

        [Required]
        public required string CorrectAnswer { get; set; }

        public string? Options { get; set; } // e.g., JSON array for multiple choices

        [StringLength(50)]
        public string? Difficulty { get; set; } // e.g., "Easy", "Medium", "Hard"

        // Navigation property
        public required Quiz Quiz { get; set; }
    }
}