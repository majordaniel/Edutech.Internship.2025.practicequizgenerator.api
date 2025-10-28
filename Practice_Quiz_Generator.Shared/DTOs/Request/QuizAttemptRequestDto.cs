using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class StartQuizRequestDto
    {
        [Required]
        public string QuizId { get; set; } = string.Empty;
    }

    public class SubmitQuizRequestDto
    {
        [Required]
        public string QuizId { get; set; } = string.Empty;

        [Required]
        public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();

        [Required]
        public int TimeSpentInSeconds { get; set; }
    }

    public class QuestionAnswerDto
    {
        [Required]
        public string QuestionId { get; set; } = string.Empty;

        [Required]
        public string SelectedAnswer { get; set; } = string.Empty;
    }
}
