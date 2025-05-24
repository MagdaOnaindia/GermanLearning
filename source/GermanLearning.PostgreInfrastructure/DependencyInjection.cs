// En tu GermanLearning.PostgreInfrastructure/DependencyInjection.cs
// O directamente en Program.cs si configuras DbContext allí.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.PostgreInfrastructure.Contexts;
using GermanLearning.PostgreInfrastructure.Repositories;
using Npgsql;
using GermanLearning.PostgreInfrastructure.Services; // <<< AÑADE ESTE USING

namespace GermanLearning.PostgreInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // --- NUEVA FORMA CON NpgsqlDataSourceBuilder ---
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            // Habilitar serialización JSON dinámica para System.Text.Json
            // Esto permite que Npgsql serialice tipos como List<string> a jsonb
            dataSourceBuilder.EnableDynamicJson(); // <<< ESTA ES LA LÍNEA CLAVE

            // Opcional: Si usas tipos enum de PostgreSQL mapeados a enums de C#
            // y quieres que Npgsql los maneje automáticamente (no es tu caso actual para WordType/Gender
            // ya que los estás mapeando a tablas de lookup, pero es bueno saberlo)
            // dataSourceBuilder.MapEnum<YourEnumType>("your_postgres_enum_type_name");

            var dataSource = dataSourceBuilder.Build(); // Construye el DataSource

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dataSource)); // Usa el DataSource configurado
                                                // options.UseNpgsql(connectionString)); // <<< FORMA ANTIGUA

            // Repositories
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IWordTypeLookupRepository, WordTypeLookupRepository>();
            services.AddScoped<IGenderLookupRepository, GenderLookupRepository>();
            services.AddSingleton<IDateTimeService, DateTimeService>();

            return services;
        }
    }
}