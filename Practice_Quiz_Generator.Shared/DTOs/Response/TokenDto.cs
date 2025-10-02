namespace Practice_Quiz_Generator.Shared.DTOs.Response
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        //    public TokenDto(string accessToken, string refreshToken)
        //    {
        //        AccessToken = accessToken;
        //        RefreshToken = refreshToken;
        //    }
    }
}
