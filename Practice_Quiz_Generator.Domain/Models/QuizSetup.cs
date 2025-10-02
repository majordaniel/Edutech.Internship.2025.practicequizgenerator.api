using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    [NotMapped]
    public class QuizSetup : BaseEntity
    {
        public string QuestionType { get; set; } 
        public int NumberOfQuestions { get; set; }
        public string QuestionSource { get; set; } //File upload
        public int Timer { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }

        public User Student { get; set; }
        public Course Course { get; set; }
    }
}
