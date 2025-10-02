using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.UOW;
using System.Text;

namespace Practice_Quiz_Generator.Extensions
{
    public static class ServiceExtentions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExamPortalContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging());
        }


        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<IStudentCourseService, StudentCourseService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileProcessingService, FileProcessingService>();
            services.AddScoped<IQuizService, QuizService>();
            //services.AddScoped<, >();
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

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:securityKey"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false, 
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:validIssuer"],
                ValidAudience = configuration["JwtSettings:validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:securityKey"])),
                //IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });
        }

    }
}
