namespace Practice_Quiz_Generator.Domain.Models
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string HOD { get; set; }
        public string FacultyId { get; set; }

        public Faculty Faculty { get; set; }
        public ICollection<User> User { get; set; }
        public ICollection<Course> Course { get; set; }
    }
}
