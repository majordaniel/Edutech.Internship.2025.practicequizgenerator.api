// // Location: Practice_Quiz_Generator.Domain.Models
// using System;
// using System.Collections.Generic;

// namespace Practice_Quiz_Generator.Domain.Models
// {
//     public class QuizAttempt : BaseEntity // Assuming you have a BaseEntity for ID
//     {
//         public Guid Id { get; set; }
//         public Guid QuizId { get; set; }
//         public string StudentId { get; set; }
//         public DateTime SubmissionTime { get; set; } = DateTime.UtcNow;
//         public int Score { get; set; }
//         public int TotalQuestions { get; set; }
        
//         // Navigation properties (if needed, this stores the details of the attempt)
//         public ICollection<QuizAttemptAnswer> Answers { get; set; }
//     }

//     public class QuizAttemptAnswer // Represents the student's answer for one question
//     {
//         public Guid Id { get; set; }
//         public Guid QuizAttemptId { get; set; }
//         public Guid QuestionId { get; set; }
//         public Guid? SelectedOptionId { get; set; }
//         public string SelectedAnswerText { get; set; }
//         public bool IsCorrect { get; set; }
//     }
// }

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    // QuizAttempt now correctly inherits and uses BaseEntity.Id (string)
    public class QuizAttempt : BaseEntity 
    {
        public string QuizId { get; set; }
        public string StudentId { get; set; }
        public DateTime SubmissionTime { get; set; } = DateTime.UtcNow;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        
        // Navigation properties
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        
        public ICollection<QuizAttemptAnswer> Answers { get; set; } = new List<QuizAttemptAnswer>();
    }
}
