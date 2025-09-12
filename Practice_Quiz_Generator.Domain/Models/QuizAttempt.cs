using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizAttempt
    {
        [Key]
        public int AttemptId { get; set; }

        [Required]
        public int QuizId { get; set; } 

        [Required]
        public int UserId { get; set; } 

        [Required]
        public int Score { get; set; }

        [Required]
        public DateTime AttemptDate { get; set; }

        [Required]
        public int TimeSpent { get; set; } 

        public required string Answer { get; set; }

        // Navigation properties
        public required Quiz Quiz { get; set; }
        public required User User { get; set; }
    }
}