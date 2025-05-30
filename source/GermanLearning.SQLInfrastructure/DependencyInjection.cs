﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GermanLearning.SQLInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using GermanLearning.Domain.Repositories.Interfaces;





namespace GermanLearning.SQLInfrastructure
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
                options.UseSqlServer(connectionString));

            // Repositories
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddSingleton<IDateTimeService, DateTimeService>();

            return services;
        }
    }

}
