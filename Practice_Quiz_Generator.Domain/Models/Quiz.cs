using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Quiz_Generator.Domain.Models
{
    [NotMapped]
    public class Quiz : BaseEntity
    {
        public string Title { get; set; }
        public bool? IsCompleted { get; set; }
        public int? TimeSpent { get; set; }
        //public string? SouceText { get; set; }
        public string QuestionType { get; set; }
        public int NumberOfQuestions { get; set; }
        public string QuestionSource { get; set; } //File upload
        public int Timer { get; set; }
        public string CourseId { get; set; }
        //public string QuizSetupId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }

        public string Content { get; set; } = string.Empty;

        //public QuizSetup QuizSetup { get; set; }
        public ICollection<QuizQuestion> QuizQuestion { get; set; }
    }
}