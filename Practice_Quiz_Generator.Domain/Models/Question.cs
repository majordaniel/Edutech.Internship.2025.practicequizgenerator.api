// using System.ComponentModel.DataAnnotations;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace Practice_Quiz_Generator.Domain.Models
// {
//    public class Question
//    {
//        [Key]
//        public Guid QuestionId { get; set; }
//        public required Guid QuizId { get; set; } 
//        public required string QuestionText { get; set; }
//        public required string CorrectAnswer { get; set; }
//        public required ICollection<Option> Options { get; set; } 
//        public string? Difficulty { get; set; } 
       
//        public string? Source { get; set; } 
//        public string? DocumentId { get; set; }

//         [ForeignKey("QuizId")]
//         public required Quiz Quiz { get; set; }
//    }
// }

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    // Keeping this class independent to maintain Guid Primary Key and Foreign Key compatibility
    public class Question 
    {
        [Key]
        public Guid Id { get; set; } // Renamed for convention (EF recognizes 'Id' as PK)
        
        public required string QuizId { get; set; } // Foreign Key to Quiz
        
        public required string QuestionText { get; set; }
        
        // Redundant with Options.IsCorrect flag, but kept if needed for text answer storage
        public required string CorrectAnswer { get; set; } 
        
        public ICollection<Option> Options { get; set; } = new List<Option>(); 
        
        public string? Difficulty { get; set; } 
        public string? Source { get; set; } 
        public string? DocumentId { get; set; }

        // Navigation properties
        [ForeignKey("QuizId")]
        public required Quiz Quiz { get; set; }
    }
}
