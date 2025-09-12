using Microsoft.EntityFrameworkCore;
using Practice_Quiz_Generator.Infrastructure.DatabaseContext;

namespace Practice_Quiz_Generator.Extensions
{
    public static class ServiceExtentions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ExamPortalContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
