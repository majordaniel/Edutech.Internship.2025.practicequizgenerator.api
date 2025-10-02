using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    [NotMapped]
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public required int QuizId { get; set; }
        public required string QuestionText { get; set; }
        public required string CorrectAnswer { get; set; }
        public string? Options { get; set; }
        public string? Difficulty { get; set; }
        public string Text { get; set; }
        public string QuestionType { get; set; }
        public string Source { get; set; }

        public string CourseId { get; set; }
        public Course Course { get; set; }
        public required Quiz Quiz { get; set; }
        //public ICollection<Option>? Option { get; set; }
    }
}
    