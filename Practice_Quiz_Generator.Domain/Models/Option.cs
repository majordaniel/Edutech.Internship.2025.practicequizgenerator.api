// // Location: Practice_Quiz_Generator.Domain.Models/Option.cs
// using System;

// namespace Practice_Quiz_Generator.Domain.Models
// {
//     public class Option
//     {
//         public Guid Id { get; set; }

//         // Foreign key back to the Question
//         public Guid QuestionId { get; set; } 

//         // The text of the answer choice (e.g., "The colon operator")
//         public string Text { get; set; }

//         // Flag indicating if this option is the correct answer
//         public bool IsCorrect { get; set; } 

//         // Navigation property
//         public Question Question { get; set; } 
//     }
// }


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    // Can inherit if you want auditing fields, but keeping it simple for now
    public class Option 
    {
        [Key]
        public Guid Id { get; set; } // Renamed for convention
        
        public Guid QuestionId { get; set; } 
        public string Text { get; set; } = string.Empty; // Initialized for required
        public bool IsCorrect { get; set; } 

        [ForeignKey("QuestionId")]
        public required Question Question { get; set; } 
    }
}
