namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizFormConfigurationDto
    {
        public required List<string> Courses { get; set; }
        public required List<string> QuestionTypes { get; set; }
        public required int MinQuestions { get; set; }
        public required int MaxQuestions { get; set; }
        public required List<string> QuestionSources { get; set; }
    }
}