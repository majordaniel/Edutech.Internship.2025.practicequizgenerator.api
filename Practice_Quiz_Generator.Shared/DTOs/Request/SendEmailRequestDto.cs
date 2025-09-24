namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class SendEmailRequestDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
