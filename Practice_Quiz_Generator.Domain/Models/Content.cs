using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        public required string Body { get; set; }

        [Required]
        public required int CourseId { get; set; }

        [Required]
        public required int CreatedId { get; set; }

        public string? ProcessedText { get; set; }

        public string? Keywords { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}