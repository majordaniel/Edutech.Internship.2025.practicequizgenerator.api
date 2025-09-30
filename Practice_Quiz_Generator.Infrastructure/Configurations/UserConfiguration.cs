using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                // Disable cascade delete to prevent multiple cascade paths
                builder.HasOne(u => u.Department)
                    .WithMany()
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict

                builder.HasOne(u => u.Faculty)
                    .WithMany()
                    .HasForeignKey(u => u.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict

                builder.HasOne(u => u.CurrentLevel)
                    .WithMany()
                    .HasForeignKey(u => u.CurrentLevelId)
                    .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict

                builder.HasOne(u => u.Role)
                    .WithMany()
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade to Restrict
            }
        }
}
