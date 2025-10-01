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
                Name = "admin",
                NormalizedName = "ADMIN",
            },
            new IdentityRole
            {
                Name = "student",
                NormalizedName = "STUDENT"
            }

            );
        }
    }
}
