using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
  public class QuizDetailsResponseDto
    {
        public string QuizId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public int TimeLimitInMinutes { get; set; }
        public List<QuestionResponseDto> Questions { get; set; } = new List<QuestionResponseDto>();
    }

    public class QuestionResponseDto
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public string Difficulty { get; set; } = string.Empty;
    }

    public class QuizResultResponseDto
    {
        public required string AttemptId { get; set; }
        public string QuizId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public double PercentageScore { get; set; }
        public int TimeSpentInSeconds { get; set; }
        public DateTime AttemptDate { get; set; }
        public List<QuestionResultDto> QuestionResults { get; set; } = new List<QuestionResultDto>();
    }

    public class QuestionResultDto
    {
        public string QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string SelectedAnswer { get; set; } = string.Empty;
        public int CorrectOptionIndex { get; set; }
        public bool IsCorrect { get; set; }
    }
}
