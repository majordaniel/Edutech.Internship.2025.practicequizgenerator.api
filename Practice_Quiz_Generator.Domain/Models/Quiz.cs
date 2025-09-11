using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        public int UserId { get; set; } // FK to User

        [Required]
        public int ContentId { get; set; } // FK to Content

        [Required]
        public DateTime GeneratedDate { get; set; } // Formerly TGeneratedDate

        [Required]
        [StringLength(50)]
        public required string Status { get; set; } // e.g., "Draft", "Active", "Completed"

        // Navigation properties
        public required User User { get; set; }
        public required Content Content { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}