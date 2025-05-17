using GermanLearning.WebApp.Components;
using GermanLearning.Application;
using GermanLearning.SQLInfrastructure;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Blazor Server

builder.Services.AddApplication(); // Tu capa Application
builder.Services.AddInfrastructure(builder.Configuration); // Tu capa Infrastructure
builder.Services.AddMudServices();


var app = builder.Build();

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
   .AddInteractiveServerRenderMode(); // Renderizado interactivo (SignalR)

app.Run();
