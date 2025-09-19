// using System.ComponentModel.DataAnnotations;

// namespace Practice_Quiz_Generator.Domain.Models
// {
//    public class Content
//    {
//        [Key]
//        public Guid ContentId { get; set; }
//        public required string Title { get; set; }
//        public required string Body { get; set; }
//        public required Guid CourseId { get; set; }
//        public required Guid CreatedId { get; set; }
//        public string? ProcessedText { get; set; }
//        public string? Keywords { get; set; }

//        public ICollection<Quiz> Quizzes { get; set; }
//    }
// }

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Content : BaseEntity // Inherits string Id PK
    {
        // REMOVED: [Key] public Guid ContentId - it's now string Id
        
        public required string Title { get; set; }
        public required string Body { get; set; }
        public required Guid CourseId { get; set; }
        public required string CreatedId { get; set; } // Assuming User ID is string
        public string? ProcessedText { get; set; }
        public string? Keywords { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
