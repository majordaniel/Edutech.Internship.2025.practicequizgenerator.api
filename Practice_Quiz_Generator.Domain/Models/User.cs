using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Practice_Quiz_Generator.Domain.Models
{
    public class User : IdentityUser<string>
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public string OtherName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;

        // Foreign Keys 
        public string DepartmentId { get; set; } = string.Empty;
        public string FacultyId { get; set; } = string.Empty;
        public string CurrentLevelId { get; set; } = string.Empty;

        // Base tracking fields
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateModified { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Status { get; set; }

        // Password Reset fields
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public Level? CurrentLevel { get; set; }
        public Department? Department { get; set; }
        public Faculty? Faculty { get; set; }
       
        
       

        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}