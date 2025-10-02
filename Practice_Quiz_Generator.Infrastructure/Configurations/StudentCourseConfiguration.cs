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
                    StudentId = "b5a4eaa2-4c9b-43e4-83b8-1e7a4520e5d9",
                    CourseId = "e7a9b6d8-0c2c-4a68-a777-4cd5aa3b68ad",
                    RegistrationDate = DateTime.UtcNow.AddDays(-60),
                    Status = "Completed"
                },
                new StudentCourse
                {
                    Id = "0c8d9a8f-9b43-46e3-8a1a-8d37e1f1f191",
                    StudentId = "b5a4eaa2-4c9b-43e4-83b8-1e7a4520e5d9",
                    CourseId = "f7e69d55-44f5-41bb-8412-c635f5c6cf88",
                    RegistrationDate = DateTime.UtcNow.AddDays(-15),
                    Status = "Registered"
                }
            );
        }
    }
}
