// GermanLearning.WebApp/Program.cs

using GermanLearning.WebApp.Components;
using GermanLearning.Application;
using GermanLearning.PostgreInfrastructure;
using MudBlazor.Services;
using GermanLearning.WebApp.Extensions;
using Microsoft.EntityFrameworkCore; // Añade este using
using GermanLearning.PostgreInfrastructure.Contexts; // Añade este using para AppDbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Blazor Server

builder.Services.AddApplication(); // Tu capa Application
builder.Services.AddInfrastructure(builder.Configuration); // Tu capa Infrastructure
builder.Services.AddMudServices();

var app = builder.Build();

// *** INICIO: APLICAR MIGRACIONES ***
// Es buena práctica hacerlo dentro de un scope para que los servicios con scoped lifetime se resuelvan correctamente
const int maxRetries = 5;
const int delayMilliseconds = 5000; // 5 seconds

for (int i = 0; i < maxRetries; i++)
{
    try
    {
        Console.WriteLine($"Attempting database operations (Attempt {i + 1}/{maxRetries})...");
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>(); // Get logger inside scope
            var context = services.GetRequiredService<AppDbContext>();

            logger.LogInformation("Applying database migrations if any are pending...");
            context.Database.Migrate(); // This will create __EFMigrationsHistory if needed and apply pending migrations.
            logger.LogInformation("Database migrations check/application complete.");
        }
        Console.WriteLine("Database operations successful."); // Renamed for clarity
        break; // Success, exit retry loop
    }
    catch (Npgsql.NpgsqlException ex) when (ex.InnerException is System.Net.Sockets.SocketException se &&
                                           (se.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionRefused ||
                                            se.SocketErrorCode == System.Net.Sockets.SocketError.HostNotFound /* No such host */))
    {
        // Log specific connection errors
        var tempScope = app.Services.CreateScope(); // Need a scope to get logger if app hasn't fully built services
        var logger = tempScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, $"Database connection error (Attempt {i + 1}/{maxRetries}). Retrying in {delayMilliseconds / 1000}s...");
        tempScope.Dispose();

        if (i == maxRetries - 1)
        {
            Console.WriteLine("Max retries reached. Failed to connect to database for migrations.");
            // Log final failure before rethrowing
            var finalScope = app.Services.CreateScope();
            var finalLogger = finalScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            finalLogger.LogError(ex, "Max retries reached. Aborting application startup due to database migration failure.");
            finalScope.Dispose();
            throw; // Rethrow after max retries
        }
        await Task.Delay(delayMilliseconds);
    }
    catch (Exception ex) // Catch other potential exceptions during migration
    {
        var tempScope = app.Services.CreateScope();
        var logger = tempScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, $"An unexpected error occurred during database setup (Attempt {i + 1}/{maxRetries}).");
        tempScope.Dispose();

        if (i == maxRetries - 1)
        {
            Console.WriteLine("Max retries reached. Failed during database setup.");
            throw; // Rethrow after max retries if it's not a connection issue but still fails
        }
        // Optionally, you might not want to retry for non-connection errors,
        // or retry fewer times. For now, this will retry for any exception during the using block.
        await Task.Delay(delayMilliseconds);
    }
}
// *** FIN: APLICAR MIGRACIONES ***

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();