using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.HasOne(qa => qa.Quiz)
                    .WithMany(q => q.QuizAttempts)
                    .HasForeignKey(qa => qa.QuizId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
        }
    }
}