using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizAttempt
    {
        [Key]
        public int AttemptId { get; set; }

        [Required]
        public int QuizId { get; set; } // FK to Quiz

        [Required]
        public int UserId { get; set; } // FK to User

        [Required]
        public int Score { get; set; }

        [Required]
        public DateTime AttemptDate { get; set; }

        [Required]
        public int TimeSpent { get; set; } // e.g., in seconds

        public required string Answer { get; set; } // e.g., JSON of user answers

        // Navigation properties
        public required Quiz Quiz { get; set; }
        public required User User { get; set; }
    }
}