using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ContentId { get; set; }

        [Required]
        public DateTime GeneratedDate { get; set; }

        [Required]
        [StringLength(50)]
        public required string Status { get; set; } 

        // Navigation properties
        public required User User { get; set; }
        public required Content Content { get; set; }
        public ICollection<Question> Questions { get; set; } 
        public ICollection<QuizAttempt> QuizAttempts { get; set; } 
    }
}