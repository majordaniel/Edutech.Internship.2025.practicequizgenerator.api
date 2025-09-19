namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuestionAnswerDto
    {
        // ID of the question
        public Guid QuestionId { get; set; } 

        // ID of the option the student selected (if multiple choice)
        public Guid? SelectedOptionId { get; set; } 

        // Could be used for text-based answers
        public string SelectedAnswerText { get; set; } 
    }
}