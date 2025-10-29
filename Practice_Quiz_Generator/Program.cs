using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Application.ServiceConfiguration.MapInitializer;
using Practice_Quiz_Generator.Application.Services.Implementations;
using Practice_Quiz_Generator.Application.Services.Interfaces;
using Practice_Quiz_Generator.Extensions;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;

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
builder.Services.ConfigureJwt(builder.Configuration);




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Apply pending migrations automatically (creates DB if missing)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ExamPortalContext>();
    ctx.Database.Migrate();
}

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