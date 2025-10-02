using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class SuperAdminConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();
            var superAdminId = "a81087b9-3543-4b8f-b9bd-4cf7a4bf5408";

            builder.HasData(
                new User
                {
                    Id = superAdminId,
                    UserName = "super-admin",
                    NormalizedUserName = "SUPER-ADMIN",
                    Email = "super-admin@admin.com",
                    NormalizedEmail = "SUPER-ADMIN@ADMIN.COM",
                    FirstName = "super-admin",
                    LastName = "super-admin",
                    OtherName = "super-admin",
                    RegistrationNumber = "SUPER-ADMIN",
                    FacultyId = "c7c6c04b-5b1f-47b1-90b0-bacb4c0d5894",    // seeded Faculty
                    DepartmentId = "b00fc3af-f38f-4451-8977-9e798db95b88", // seeded Department
                    CurrentLevelId = "b61c1f26-65ef-4ef4-8c0c-bf6f47b3e3d1", // seeded Level
                    EmailConfirmed = true,
                    IsActive = true,
                    PasswordHash = hasher.HashPassword(null, "SuperAdmin#123"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    DateCreated = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }


    }
}
