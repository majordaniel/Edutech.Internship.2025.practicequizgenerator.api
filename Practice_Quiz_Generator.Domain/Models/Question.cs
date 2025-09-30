using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class Question
    {
        public string Text { get; set; }      
        public string QuestionType { get; set; }
        public string Source { get; set; }

        public string CourseId { get; set; }          
        public Course Course { get; set; }
namespace Practice_Quiz_Generator.Domain.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public required int QuizId { get; set; }
        public required string QuestionText { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Options { get; set; }
        public string? Difficulty { get; set; }

        public ICollection<Option>? Option { get; set; }
    }
}
        public required Quiz Quiz { get; set; }
    }
}