namespace Practice_Quiz_Generator.Domain.Models
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; } = false;

        public User User { get; set; }
    }
}
