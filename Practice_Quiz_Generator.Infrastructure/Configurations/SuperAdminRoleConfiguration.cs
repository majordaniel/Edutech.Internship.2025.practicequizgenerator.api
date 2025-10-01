using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class SuperAdminRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {
                UserId = "a81087b9-3543-4b8f-b9bd-4cf7a4bf5408",
                RoleId = "9d3730d8-d790-4158-83b8-e5d96067359d",

            }
                );
        }
    }
}
