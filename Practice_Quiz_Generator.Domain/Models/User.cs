using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public required string Role { get; set; }

        // Navigation properties for 1:N relationships
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}