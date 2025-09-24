using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasData(
                // Computer Science (DepartmentId: b00fc3af-f38f-4451-8977-9e798db95b88)
                new Course
                {
                    Id = "e7a9b6d8-0c2c-4a68-a777-4cd5aa3b68ad",
                    Title = "Introduction to Programming",
                    Code = "CSC 101",
                    CreditUnit = 3,
                    Semester = "First",
                    LevelId = "b61c1f26-65ef-4ef4-8c0c-bf6f47b3e3d1",
                    DepartmentId = "b00fc3af-f38f-4451-8977-9e798db95b88"
                },
                new Course
                {
                    Id = "f7e69d55-44f5-41bb-8412-c635f5c6cf88",
                    Title = "Discrete Mathematics",
                    Code = "MTH 101",
                    CreditUnit = 2,
                    Semester = "First",
                    LevelId = "b61c1f26-65ef-4ef4-8c0c-bf6f47b3e3d1",
                    DepartmentId = "b00fc3af-f38f-4451-8977-9e798db95b88"
                }

            );
        }
    }
}
