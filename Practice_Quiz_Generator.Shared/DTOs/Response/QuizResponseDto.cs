namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizResponseDto
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<QuizQuestionResponseDto> Questions { get; set; } = new List<QuizQuestionResponseDto>();
    }
}
