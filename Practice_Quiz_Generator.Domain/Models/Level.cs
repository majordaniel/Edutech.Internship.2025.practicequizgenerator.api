namespace Practice_Quiz_Generator.Domain.Models
{
    public class Level : BaseEntity
    {
        public string LevelId { get; set; }
        public string Code { get; set; }
        public int Year { get; set; }

        public ICollection<User> User { get; set; }
        public ICollection<Course> Course { get; set; }
    }
}

