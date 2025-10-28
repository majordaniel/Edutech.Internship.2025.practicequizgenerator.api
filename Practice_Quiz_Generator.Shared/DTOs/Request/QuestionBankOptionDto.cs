namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class QuestionBankOptionDto
    {
        public string OptionLabel { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
