using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public required string FullName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}