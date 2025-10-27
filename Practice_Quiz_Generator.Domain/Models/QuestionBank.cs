using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuestionBank : BaseEntity
    {
        public string Text { get; set; }      
        public string? QuestionType { get; set; }
        public string CorrectAnswer { get; set; }
        public string Source { get; set; }

        public string CourseId { get; set; }          
        public Course Course { get; set; }

        public ICollection<QuestionBankOption>? Option { get; set; }
    }
}