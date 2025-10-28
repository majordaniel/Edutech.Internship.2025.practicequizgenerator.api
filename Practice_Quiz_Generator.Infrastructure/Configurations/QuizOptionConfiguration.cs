using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOption>
    {
        public void Configure(EntityTypeBuilder<QuizOption> builder)
        {
            // Relationship with QuizQuestion
            builder.HasOne(qo => qo.QuizQuestion)
                   .WithMany(qq => qq.QuizOption)
                   .HasForeignKey(qo => qo.QuizQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);  // Delete Options when QuizQuestion is deleted
        }
    }
}