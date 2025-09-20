using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new IdentityRole
            {
                Name = "super-admin",
                NormalizedName = "SUPER-ADMIN",
            },
            new IdentityRole
            {
                Id = "9d3730d8-d790-4158-83b8-e5d96067359d",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "Student",
                NormalizedName = "STUDENT"
            }

            );
        }
    }
}
