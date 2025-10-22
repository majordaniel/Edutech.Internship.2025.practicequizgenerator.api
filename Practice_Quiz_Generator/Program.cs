using Microsoft.OpenApi.Models;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MapInitializer;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Extensions;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureDependencyInjection();
builder.Services.AddAutoMapper(cfg => { },
    typeof(MappingProfile)
);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureCors();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
