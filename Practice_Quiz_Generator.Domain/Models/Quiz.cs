using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Quiz : BaseEntity
    {
        public bool IsCompleted { get; set; }
        public int TimeSpent { get; set; }
        public string QuizSetupId { get; set; }
        public QuizSetup QuizSetup { get; set; }
        public ICollection<QuizQuestion> QuizQuestion { get; set; }
    }
}