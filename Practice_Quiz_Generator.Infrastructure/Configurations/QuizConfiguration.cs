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
                   .WithMany(u => u.Quizzes)  // Matches User's ICollection<Quiz>
                   .HasForeignKey(q => q.UserId)
                   .OnDelete(DeleteBehavior.Restrict)  // Don't delete user if quiz deleted
                   .IsRequired();

            builder.HasOne(q => q.Course)
                   .WithMany()  // If Course has no backref, empty
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            // Add for QuizQuestions if not convention-based
            builder.HasMany(q => q.QuizQuestion)
                   .WithOne(qq => qq.Quiz)
                   .HasForeignKey(qq => qq.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);  // Delete questions with quiz
        }
    }
}