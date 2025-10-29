// Shared/DTOs/Response/QuizUserStatsDto.cs
namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizUserStatsDto
    {
        public int TotalQuizzes { get; set; }
        public double AverageScore { get; set; }          // % (0-100)
        public double? LastQuizScore { get; set; }        // % (null if no attempts)
        public double AverageTimeSpent { get; set; }      // seconds
    }
}