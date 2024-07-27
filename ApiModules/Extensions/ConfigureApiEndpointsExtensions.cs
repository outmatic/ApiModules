using ApiModules.Abstractions;
using ApiModules.Behaviours;
using ApiModules.Transactions;
using ApiModules.Transactions.Abstractions;
using ApiModules.Validation;
using FluentValidation;

namespace ApiModules.Extensions;

public static class ConfigureApiEndpointsExtensions
{
    private static readonly List<IApiModule> RegisteredModules = new();
    
    public static IServiceCollection AddApiEndpoints<T>(this IServiceCollection services)
    {
        services.AddMediatr<T>()
            .AddEndpointsApiExplorer()
            .AddProblemDetails();

        var modules = DiscoverModules(typeof(T));

        foreach (var module in modules)
        {
            module.RegisterApiModule(services);
            RegisteredModules.Add(module);
        }

        return services;
    }

    public static IApplicationBuilder UseApiEndpoints(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        
        var group = app.MapGroup("")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);

        foreach (var module in RegisteredModules)
            module.MapApiModule(group);

        return app;
    }

    private static IEnumerable<IApiModule> DiscoverModules(Type t)
        => t.Assembly
            .GetTypes()
            .Where(p => p is
            {
                IsClass: true,
                IsAbstract: false
            } && p.IsAssignableTo(typeof(IApiModule)))
            .Select(Activator.CreateInstance)
            .Cast<IApiModule>();
    
    private static IServiceCollection AddMediatr<T>(this IServiceCollection services)
    {
        if (services.All(x => x.ServiceType != typeof(ITransactionContext)))
            services.AddScoped<ITransactionContext, EmptyTransactionContext>();

        services.AddValidatorsFromAssemblyContaining<T>(ServiceLifetime.Singleton);
        ValidatorOptions.Global.PropertyNameResolver = ApiModulePropertyNameResolver.ResolvePropertyName;
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(T).Assembly);
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        return services;
    }
}