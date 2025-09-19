namespace Practice_Quiz_Generator.Domain.Models
{
    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int CreditUnit { get; set; }
        public string Semester { get; set; }
        public string LevelId { get; set; }
        public string DepartmentId { get; set; }

        public Level Level { get; set; }
        public Department Department { get; set; }
    }
}
