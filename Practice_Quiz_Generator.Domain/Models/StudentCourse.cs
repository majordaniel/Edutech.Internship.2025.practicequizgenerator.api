namespace Practice_Quiz_Generator.Domain.Models
{
    public class StudentCourse : BaseEntity
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
       
        //Note --> I added status to BaseEntity, It's applicable here: eg: Registered, Completed, Dropped etc
    }
}
