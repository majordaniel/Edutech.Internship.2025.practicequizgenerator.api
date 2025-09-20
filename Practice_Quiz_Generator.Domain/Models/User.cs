using Microsoft.AspNetCore.Identity;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string RegistrationNumber { get; set; }
        public string? DepartmentId { get; set; }
        public string? FacultyId { get; set; }
        public string? CurrentLevelId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? Status { get; set; }
      
        public Level CurrentLevel { get; set; }
        public Department Department { get; set; }
        public Faculty Faculty { get; set; }

        //public ICollection<StudentCourse> StudentCourses { get; set; }

        //public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        //public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}