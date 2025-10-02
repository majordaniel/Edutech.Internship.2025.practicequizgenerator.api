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
             

                builder.HasOne(u => u.Department)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict); 

                builder.HasOne(u => u.Faculty)
                    .WithMany(f => f.Users)
                    .HasForeignKey(u => u.FacultyId)
                    .OnDelete(DeleteBehavior.Restrict); 

                builder.HasOne(u => u.CurrentLevel)
                    .WithMany()
                    .HasForeignKey(u => u.CurrentLevelId)
                    .OnDelete(DeleteBehavior.Restrict);

               
            }
        }
}
