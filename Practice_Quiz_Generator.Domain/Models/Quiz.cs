using System;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
   public class Quiz
   {
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
   }
}