using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasOne(q => q.User)
                   .WithMany()
                   .HasForeignKey(q => q.UserId)
                   .IsRequired();

            builder.HasOne(q => q.Content)
                   .WithMany()
                   .HasForeignKey(q => q.ContentId)
                   .IsRequired();
            
        }
    }
}