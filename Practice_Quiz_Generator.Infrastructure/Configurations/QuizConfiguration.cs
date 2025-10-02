//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Practice_Quiz_Generator.Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Practice_Quiz_Generator.Infrastructure.Configurations
//{
//    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
//    {
//        public void Configure(EntityTypeBuilder<Quiz> builder)
//        {
//            builder.HasOne(q => q.User)
//                .WithMany()
//                .HasForeignKey("UserId1")
//                .OnDelete(DeleteBehavior.Restrict);

//            builder.HasOne(q => q.Content)
//                .WithMany()
//                .HasForeignKey(q => q.ContentId)
//                .OnDelete(DeleteBehavior.Cascade);
//        }
//    }

//}
