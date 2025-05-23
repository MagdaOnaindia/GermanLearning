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
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Applying database migrations...");
            context.Database.Migrate(); // Aplica migraciones pendientes
            Console.WriteLine("Database migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations to apply.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        // Considerar si la aplicación debe fallar al arrancar si las migraciones no se pueden aplicar.
        // throw; // Descomentar si quieres que la app falle si la migración no tiene éxito.
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