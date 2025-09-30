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
                Id = "11111111-1111-1111-1111-111111111111",
                Name = "super-admin",
                NormalizedName = "SUPER-ADMIN",
            },
            new IdentityRole
            {
                Id = "22222222-2222-2222-2222-222222222222",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "33333333-3333-3333-3333-333333333333",
                Name = "Student",
                NormalizedName = "STUDENT"
            }

            );
        }
    }
}
