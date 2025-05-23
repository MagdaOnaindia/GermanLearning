using GermanLearning.PostgreInfrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GermanLearning.WebApp.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigration(this IApplicationBuilder application)
        {
            using IServiceScope scope = application.ApplicationServices.CreateScope();
            using AppDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
