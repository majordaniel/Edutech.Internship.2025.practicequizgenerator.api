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
            builder.HasData(
            new User
            {
                Id = "a81087b9-3543-4b8f-b9bd-4cf7a4bf5408",
                UserName = "super-admin",        
                Email = "super-admin@admin.com", 
                FirstName = "super-admin",
                LastName = "super-admin",
                OtherName = "super-admin",
                RegistrationNumber = "SUPER-ADMIN",
                FacultyId = "ADMIN-FACULTY-ID",
                DepartmentId = "ADMIN-DEPARTMENT-ID",
                CurrentLevelId = "ADMIN-LEVEL-ID",
                NormalizedUserName = "SUPER-ADMIN",
                NormalizedEmail = "SUPER-ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "SuperAdmin#123"),
                SecurityStamp = Guid.NewGuid().ToString(),
            }
             );
        }


    }
}
