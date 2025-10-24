namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuestionBankOption : BaseEntity
    {
        public string OptionLabel { get; set; } // A, B, C, D
        public string OptionText { get; set; } 
        public bool IsCorrect { get; set; }

        public string QuestionBankId { get; set; }
        public QuestionBank QuestionBank { get; set; } 
    }
}
