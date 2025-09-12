using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Infrastructure.DatabaseContext
{
    public class ExamPortalContext : DbContext
    {
        public DbSet<Content> Contents { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }

        public ExamPortalContext(DbContextOptions<ExamPortalContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.User)
                .WithMany(u => u.Quizzes)
                .HasForeignKey(q => q.UserId);

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Content)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.ContentId);

            modelBuilder.Entity<Question>()
                .HasOne(qn => qn.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(qn => qn.QuizId);

            modelBuilder.Entity<QuizAttempt>()
                .HasOne(qa => qa.Quiz)
                .WithMany(q => q.QuizAttempts)
                .HasForeignKey(qa => qa.QuizId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<QuizAttempt>()
                .HasOne(qa => qa.User)
                .WithMany(u => u.QuizAttempts)
                .HasForeignKey(qa => qa.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}