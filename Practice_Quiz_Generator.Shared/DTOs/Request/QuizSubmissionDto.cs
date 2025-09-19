namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuizSubmissionDto
    {
        // Unique ID for the quiz being attempted
        public Guid QuizId { get; set; }

        // ID of the student submitting the quiz
        public string StudentId { get; set; } 

        // List of the student's selected answers
        public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();
    }
}