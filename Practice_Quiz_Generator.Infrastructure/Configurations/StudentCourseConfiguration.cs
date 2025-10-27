using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasData(
   
                new StudentCourse
                {
                    Id = "29f9dc7c-5b47-4b5d-8f21-0913e54fbb5d",
                    StudentId = "fb901b80-434b-4d22-a91e-6658ac190f1d", 
                    CourseId = "e7a9b6d8-0c2c-4a68-a777-4cd5aa3b68ad",  
                    RegistrationDate = DateTime.UtcNow.AddDays(-60),
                    Status = "Completed"
                }
            );
        }
    }
}
