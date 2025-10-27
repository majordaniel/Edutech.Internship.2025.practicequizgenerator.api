using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizAttempt : BaseEntity
    {
        public string QuizId { get; set; }
        public required string UserId { get; set; }
        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }
        public int TimeSpent { get; set; }
        public required string Answer { get; set; }

        public required Quiz Quiz { get; set; }
        public  User? User { get; set; }
        
        
    }
}