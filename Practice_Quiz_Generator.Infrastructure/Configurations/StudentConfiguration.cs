using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(

                new User
                {
                    Id = "b5a4eaa2-4c9b-43e4-83b8-1e7a4520e5d9",
                    UserName = "mary.okafor",
                    NormalizedUserName = "MARY.OKAFOR",
                    Email = "mary.okafor@example.com",
                    NormalizedEmail = "MARY.OKAFOR@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAENz5kl9o32MQEmc5cf3XKf+dYV7rm0UQkg=",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    FirstName = "Mary",
                    LastName = "Okafor",
                    OtherName = "Chinyere",
                    RegistrationNumber = "SCI/2021/045",
                    FacultyId = "c7c6c04b-5b1f-47b1-90b0-bacb4c0d5894", // Faculty of Science
                    DepartmentId = "b00fc3af-f38f-4451-8977-9e798db95b88", // Computer Science
                    CurrentLevelId = "b61c1f26-65ef-4ef4-8c0c-bf6f47b3e3d1" // 100 Level
                }
                );
        }
    }
}
