// using System;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace Practice_Quiz_Generator.Domain.Models
// {
//    public class Quiz
//    {
//        [Key]
//        public Guid QuizId { get; set; }
//        public string UserId { get; set; }
//        public Guid ContentId { get; set; }
//        public DateTime GeneratedDate { get; set; }
//        public required string Status { get; set; } 

//        public required User User { get; set; }

//        [ForeignKey("ContentId")]
//        public required Content Content { get; set; }
//        public ICollection<Question> Questions { get; set; } 
//     //    public ICollection<QuizAttempt> QuizAttempts { get; set; } 
//    }
// }

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Practice_Quiz_Generator.Domain.Models
{
    // Inherit from BaseEntity for common fields and the primary key
    public class Quiz : BaseEntity 
    {
        // REMOVED: [Key] public Guid QuizId
        // The PK is inherited as 'public string Id' from BaseEntity

        // Foreign Key to User (assuming User.Id is a string, which is common with Identity)
        public string UserId { get; set; }

        // Foreign Key to Content (Guid is fine if Content entity uses Guid for PK)
        public string ContentId { get; set; } 
        
        public DateTime GeneratedDate { get; set; }
        
        // Removed redundant Status property since it's in BaseEntity
        
        // Navigation properties
        [ForeignKey("UserId")] // Explicitly link User to UserId
        public User? User { get; set; }

        [ForeignKey("ContentId")]
        public Content? Content { get; set; }
        
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
