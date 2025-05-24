using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using GermanLearning.Domain.Repositories.Interfaces;

using GermanLearning.PostgreInfrastructure.Contexts;
using GermanLearning.PostgreInfrastructure.Repositories;
using GermanLearning.PostgreInfrastructure.Services;



namespace GermanLearning.PostgreInfrastructure
{


    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configuración de EF Core con SQL Server
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Repositories
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddSingleton<IDateTimeService, DateTimeService>();

            return services;
        }
    }

}
