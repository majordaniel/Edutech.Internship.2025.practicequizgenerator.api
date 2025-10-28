namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuestionByCourseResponseDto
    {
        public string CourseTitle { get; set; }
        public int TotalQuestions { get; set; }
        public List<QuestionBankResponseDto> Questions { get; set; }
    }
}
