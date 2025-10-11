using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            // Primary key from BaseEntity (Id)
            builder.HasKey(qa => qa.Id);  // Ensure Id is the primary key

            // Composite key as unique constraint (optional, if you want to enforce uniqueness)
            builder.HasIndex(qa => new { qa.QuizId, qa.UserId }).IsUnique();

            builder.HasOne(qa => qa.Quiz)
                   .WithMany(q => q.QuizAttempts)
                   .HasForeignKey(qa => qa.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(qa => qa.User)
                   .WithMany(u => u.QuizAttempts)
                   .HasForeignKey(qa => qa.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}