namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuizSubmissionRequestDto
    {
        public string QuizId { get; set; }
        public string UserId { get; set; }
        public int TimeSpent { get; set; }  // In seconds
        public List<UserAnswerDto> Answers { get; set; } = new List<UserAnswerDto>();
    }

    public class UserAnswerDto
    {
        public string QuizQuestionId { get; set; }
        public int SelectedOptionIndex { get; set; }  // 0-based index
        public string SelectedOptionId { get; set; }  // Optional, if using Ids
    }
}