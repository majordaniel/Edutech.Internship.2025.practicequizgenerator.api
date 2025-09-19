namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class QuizSetupResponseDto
    {
        public List<CourseDto> Courses { get; set; }
        public List<string> QuestionTypes { get; set; }
        public List<string> Sources { get; set; }
        public int MinQuestions { get; set; }
        public int MaxQuestions { get; set; }
    }

    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}