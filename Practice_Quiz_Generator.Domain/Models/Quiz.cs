using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Quiz : BaseEntity
    {
        public bool IsCompleted { get; set; }
        public string QuizSetupId { get; set; }

        public QuizSetup QuizSetup { get; set; }
        //public User User { get; set; }
        public ICollection<Question> Question { get; set; }

        //public required Content Content { get; set; }
        //public ICollection<Question> Questions { get; set; }
        //public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}