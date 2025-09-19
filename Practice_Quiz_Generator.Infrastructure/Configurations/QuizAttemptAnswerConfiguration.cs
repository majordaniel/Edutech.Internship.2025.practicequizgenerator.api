using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
        {
            // Relationship: QuizAttemptAnswer to QuizAttempt
            builder.HasOne(qaa => qaa.QuizAttempt)
                    .WithMany() 
                    .HasForeignKey(qaa => qaa.QuizAttemptId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

            builder.HasOne(qaa => qaa.Question)
                    .WithMany() 
                    .HasForeignKey(qaa => qaa.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
        }
    }
}