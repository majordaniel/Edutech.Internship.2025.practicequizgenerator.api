using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.HasData(
                new Faculty
                {
                    Id = "c7c6c04b-5b1f-47b1-90b0-bacb4c0d5894",
                    Name = "Faculty of Science",
                    Code = "SCI",
                    Dean = "Prof. Chinyere Okafor",
                    Description = "Includes Physics, Chemistry, Biology, and Computer Science departments."
                }
            );
        }
    }
}
