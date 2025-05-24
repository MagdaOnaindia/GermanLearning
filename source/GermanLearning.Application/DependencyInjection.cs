using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using GermanLearning.Application.Interfaces;
using GermanLearning.Application.Services;
using GermanLearning.Domain.Repositories.Interfaces;
using System.Reflection;
using MediatR;

namespace GermanLearning.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Services
        services.AddScoped<IWordService, WordService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<IWordTypeLookupService, WordTypeLookupService>();
        services.AddScoped<IGenderLookupService, GenderLookupService>();
        // Register Validators (FluentValidation)
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register MediatR (if using)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        // Register all validators from assembly
        services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly(),
            includeInternalTypes: true);

        return services;
    }
}