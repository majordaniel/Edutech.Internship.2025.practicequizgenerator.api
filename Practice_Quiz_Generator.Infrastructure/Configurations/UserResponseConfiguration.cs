using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.Configurations
{
    public class UserResponseConfiguration : IEntityTypeConfiguration<UserResponse>
    {
        public void Configure(EntityTypeBuilder<UserResponse> builder)
        {
            builder.HasOne(ur => ur.Quiz)
                   .WithMany(q => q.UserResponses)
                   .HasForeignKey(ur => ur.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.QuizQuestion)
                   .WithMany(qq => qq.UserResponses)
                   .HasForeignKey(ur => ur.QuizQuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ur => ur.SelectedOption)
                   .WithMany()
                   .HasForeignKey(ur => ur.SelectedOptionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserResponses)
                   .HasForeignKey(ur => ur.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}