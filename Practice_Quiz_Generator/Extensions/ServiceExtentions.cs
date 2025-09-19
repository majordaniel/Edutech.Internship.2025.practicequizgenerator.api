using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.UOW;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Practice_Quiz_Generator.Domain.Models;

namespace Practice_Quiz_Generator.Extensions
{
    public static class ServiceExtentions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExamPortalContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Practice_Quiz_Generator.Infrastructure")
                )
                .EnableSensitiveDataLogging()
            );
        }


        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<IQuizValidationService, QuizValidationService>();
            services.AddScoped<IQuizGenerationService, QuizGenerationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentCourseService, StudentCourseService>();
            services.AddScoped<IQuizSetupService, QuizSetupService>();
            services.AddScoped<IQuizSubmissionService, QuizSubmissionService>();

            services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
            services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod());
            });
        }
        
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(i =>
            {
                i.Password.RequireDigit = true;
                i.Password.RequireLowercase = false;
                i.Password.RequireUppercase = false;
                i.Password.RequireNonAlphanumeric = false;
                i.Password.RequiredLength = 8;
                i.User.RequireUniqueEmail = true;
                i.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ExamPortalContext>()
            .AddDefaultTokenProviders();
        }
    }
}
