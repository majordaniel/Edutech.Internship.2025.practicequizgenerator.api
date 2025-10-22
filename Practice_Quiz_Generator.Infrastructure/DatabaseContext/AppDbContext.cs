using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Domain.Models;


namespace Practice_Quiz_Generator.Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ImportJob> ImportJobs { get; set; }
        public DbSet<ImportJobLog> ImportJobLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: configure relationships or table names here
            // Example:
            // modelBuilder.Entity<Question>()
            //     .HasMany(q => q.Option)
            //     .WithOne(o => o.QuizQuestion)
            //     .HasForeignKey(o => o.QuizQuestionId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
