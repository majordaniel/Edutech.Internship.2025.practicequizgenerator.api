namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class GenerateQuizFromBankRequestDto
    {
        public string CourseId { get; set; }
        public int NumberOfQuestions { get; set; } = 10;
        public string? Difficulty { get; set; } // "Easy", "Medium", "Hard"
    }

}
