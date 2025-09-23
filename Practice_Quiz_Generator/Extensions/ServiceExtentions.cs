using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;

namespace Practice_Quiz_Generator.Extensions
{
    public static class ServiceExtentions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExamPortalContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

     
            public static void ConfigureDependencyInjection(this IServiceCollection services)
            {
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            //services.AddScoped<ICourseService, ICourseService>();
           

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
    }
}
