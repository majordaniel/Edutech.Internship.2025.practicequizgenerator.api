namespace Practice_Quiz_Generator.Shared.DTOs.Request
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
