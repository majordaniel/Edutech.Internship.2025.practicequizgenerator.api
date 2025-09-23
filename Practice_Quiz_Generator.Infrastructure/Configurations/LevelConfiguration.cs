using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.HasData(
                new Level
                {
                    Id = "b61c1f26-65ef-4ef4-8c0c-bf6f47b3e3d1",
                    Code = "100",
                    Year = 1
                },
                new Level
                {
                    Id = "c94b6c25-715f-41bc-8c87-1cf06d6bb3e9",
                    Code = "200",
                    Year = 2
                },
                new Level
                {
                    Id = "a3d8e3c1-321f-4f39-9636-282b38caaef4",
                    Code = "300",
                    Year = 3
                },
                new Level
                {
                    Id = "e86d79f4-729c-45c2-a2f7-9fcf3b4b16d3",
                    Code = "400",
                    Year = 4
                },
                new Level
                {
                    Id = "d5294f9b-64f2-4d62-9a0a-6a0fcdcc8db1",
                    Code = "500",
                    Year = 5
                },
                new Level
                {
                    Id = "f873e1ad-1c90-4a69-b6e4-01c2a8e5f916",
                    Code = "600",
                    Year = 6
                }
    );
        }
    }
}
