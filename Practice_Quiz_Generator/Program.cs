using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MapInitializer;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Domain.Models;
using Practice_Quiz_Generator.Extensions;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;

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


builder.Services.AddScoped<IAuthService, AuthService>();
// Add services to the container.
builder.Services.ConfigureDatabase(builder.Configuration);

builder.Services.AddAutoMapper(cfg => { },
    typeof(MappingProfile)
);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureCors();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Practice Quiz API", Version = "v1" });

//    // Handle file uploads in Swagger
//    c.MapType<IFormFile>(() => new OpenApiSchema
//    {
//        Type = "string",
//        Format = "binary"
//    });
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
