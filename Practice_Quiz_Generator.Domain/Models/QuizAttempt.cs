using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    [NotMapped]
    public class QuizAttempt
    {
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        //public DateTime AttemptDate { get; set; }
        public int TimeSpent { get; set; }
        public required string Answer { get; set; }

        public required Quiz Quiz { get; set; }
        public required User User { get; set; }
    }
}