using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            // Relationship with Quiz
            builder.HasOne(qq => qq.Quiz)
                   .WithMany(q => q.QuizQuestion)
                   .HasForeignKey(qq => qq.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);  // Delete QuizQuestions when Quiz is deleted

            // Relationship with QuizOption
            builder.HasMany(qq => qq.QuizOption)
                   .WithOne(qo => qo.QuizQuestion)
                   .HasForeignKey(qo => qo.QuizQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);  // Delete Options when QuizQuestion is deleted

            // Relationship with UserResponse
            builder.HasMany(qq => qq.UserResponses)
                   .WithOne(ur => ur.QuizQuestion)
                   .HasForeignKey(ur => ur.QuizQuestionId)
                   .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion if responses exist
        }
    }
}