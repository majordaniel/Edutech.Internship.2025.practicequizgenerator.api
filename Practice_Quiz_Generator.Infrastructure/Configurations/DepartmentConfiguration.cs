using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasOne(d => d.Faculty)
                   .WithMany(f => f.Departments)
                   .HasForeignKey(d => d.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                // Faculty of Science
                new Department
                {
                    Id = "b00fc3af-f38f-4451-8977-9e798db95b88",
                    Name = "Department of Computer Science",
                    Code = "CSC",
                    Description = "Focuses on software development, AI, and computer systems.",
                    HOD = "Dr. Kudirat Bello",
                    FacultyId = "c7c6c04b-5b1f-47b1-90b0-bacb4c0d5894"
                }
            );
        }
    }
}
