using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizGenerationRequestDto
    {
        [Required]
        public int SelectedCourseId { get; set; }

        [Required]
        public string QuestionType { get; set; }

        [Required]
        [Range(5, 50, ErrorMessage = "Number of questions must be between 5 and 50.")]
        public int NumberOfQuestions { get; set; }

        [Required]
        public string Source { get; set; }

        // Conditionally required field based on the 'Source'
        public int? DocumentId { get; set; }
    }
}