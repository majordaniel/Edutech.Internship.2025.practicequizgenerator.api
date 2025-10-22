namespace Practice_Quiz_Generator.Domain.Models
{
    public class QuizQuestion
    {   
        public int Id { get; set; }
        public string Text { get; set; }
        public int ScoreValue { get; set; }
        public string QuizId { get; set; }
        //public string? QuestionId { get; set; } 

        public Quiz Quiz { get; set; }
        public ICollection<Option> Option { get; set; }
        public ICollection<UserResponse> UserResponses { get; set; }
        //public Question? Question { get; set; }
    }
}

