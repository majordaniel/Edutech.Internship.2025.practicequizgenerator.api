namespace Practice_Quiz_Generator.Domain.Models
{
    public class Faculty : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Dean { get; set; }
        public string Description { get; set; }

        public ICollection<Department>? Department { get; set; }
        public ICollection<User>? User { get; set; }
    }
}
