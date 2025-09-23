using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
   public class Content
   {
       [Key]
       public int ContentId { get; set; }
       public required string Title { get; set; }
       public required string Body { get; set; }
       public required int CourseId { get; set; }
       public required int CreatedId { get; set; }
       public string? ProcessedText { get; set; }
       public string? Keywords { get; set; }

       public ICollection<Quiz> Quizzes { get; set; }
   }
}