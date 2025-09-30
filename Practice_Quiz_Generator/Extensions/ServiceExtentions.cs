using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Infrastructure.Configurations;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.Repositories.Implementations;
using Practice_Quiz_Generator.Infrastructure.Repositories.Interfaces;
using Practice_Quiz_Generator.Infrastructure.UOW;
using System.Text;

namespace Practice_Quiz_Generator.Extensions
{
    public static class ServiceExtentions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExamPortalContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // JWT Configuration
            var jwtSettings = new JwtSettingsConfiguration();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.Configure<JwtSettingsConfiguration>(configuration.GetSection("JwtSettings"));

            // JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Authorization
            services.AddAuthorization();

            // Repository registrations
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IFacultyRepository, FacultyRepository>();
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();

            // Service registrations
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IFacultyService, FacultyService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<IStudentCourseService, StudentCourseService>();

            // CORS configuration
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // API Controllers
            services.AddControllers();

            // Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}



//        public static void ConfigureDependencyInjection(this IServiceCollection services)
//        {
//            services.AddScoped<IUnitOfWork, UnitOfWork>();
//            services.AddScoped<IFacultyService, FacultyService>();
//            services.AddScoped<IDepartmentService, DepartmentService>();
//            services.AddScoped<ICourseService, CourseService>();
//            services.AddScoped<ILevelService, LevelService>();
//            services.AddScoped<IStudentCourseService, StudentCourseService>();
//            services.AddScoped<IEmailService, EmailService>();
//            services.AddScoped<IAuthService, AuthService>();
//            services.AddScoped<IJwtService, IJwtService>();

//        }


//        public static void ConfigureIdentity(this IServiceCollection services)
//        {
//            var builder = services.AddIdentity<User, IdentityRole>(i =>
//            {
//                i.Password.RequireDigit = true;
//                i.Password.RequireLowercase = false;
//                i.Password.RequireUppercase = false;
//                i.Password.RequireNonAlphanumeric = false;
//                i.Password.RequiredLength = 8;
//                i.User.RequireUniqueEmail = true;
//                i.SignIn.RequireConfirmedEmail = true;
//            })
//            .AddEntityFrameworkStores<ExamPortalContext>()
//            .AddDefaultTokenProviders();
//        }

//        public static void ConfigureCors(this IServiceCollection services)
//        {
//            services.AddCors(option =>
//            {
//                option.AddPolicy("CorsPolicy", builder => builder
//                .AllowAnyHeader()
//                .AllowAnyOrigin()
//                .AllowAnyMethod());
//            });
//        }
    

