using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    [NotMapped]
    public class Quiz : BaseEntity
    {
        public bool IsCompleted { get; set; }
        public int TimeSpent { get; set; }
        public string QuizSetupId { get; set; }
        public QuizSetup QuizSetup { get; set; }

        [Key]
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int ContentId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public required string Status { get; set; }

        public required User User { get; set; }
        public required Content Content { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
        public ICollection<QuizQuestion> QuizQuestion { get; set; }
    }
}
