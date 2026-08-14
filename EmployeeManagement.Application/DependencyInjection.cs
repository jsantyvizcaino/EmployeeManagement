using EmployeeManagement.Application.Common.Behaviors;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            ServiceLifetime.Scoped,
            includeInternalTypes: true);

        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddSingleton(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        services.AddSingleton(
            typeof(IPipelineBehavior<,>),
            typeof(PerformanceBehavior<,>));
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
