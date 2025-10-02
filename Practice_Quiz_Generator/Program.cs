using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MapInitializer;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Extensions;
using Practice_Quiz_Generator.Infrastructure;
using Practice_Quiz_Generator.Infrastructure.Configurations;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Practice_Quiz_Generator.Infrastructure.UOW;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ExamPortalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    //configure password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ExamPortalContext>()
.AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettingsConfiguration>();

if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
{
    throw new InvalidOperationException("JWT settings are missing or incomplete in appsettings.json");
}

var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

//builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidIssuer = jwtSettings.Issuer,
//        ValidateAudience = true,
//        ValidAudience = jwtSettings.Audience,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero
//    };
//});

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = true,
    ValidIssuer = jwtSettings.Issuer,
    ValidateAudience = true,
    ValidAudience = jwtSettings.Audience,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.Configure<JwtSettingsConfiguration>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.ConfigureDependencyInjection();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
builder.Services.ConfigureCors();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRolesAsync(roleManager);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


