using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GermanLearning.Infrastructure.Persistence.Contexts;
using GermanLearning.Infrastructure.Persistence.Repositories;
using GermanLearning.Infrastructure.Services;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.Infrastructure.Configurations;


namespace GermanLearning.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // MongoDB Configuration - Corrected version
        services.Configure<MongoConfig>(configuration.GetSection("MongoDB"));

        // Alternative if the above doesn't work:
        //services.Configure<MongoConfig>(options =>
        //    configuration.GetSection("MongoDB").Bind(options));

        services.AddSingleton<MongoDbContext>();

        // Repositories
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        // Services
        services.AddSingleton<IDateTimeService, DateTimeService>();

        return services;
    }
}