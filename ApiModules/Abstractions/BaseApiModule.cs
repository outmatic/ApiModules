namespace ApiModules.Abstractions;

public abstract class BaseApiModule : IApiModule
{
    public virtual IServiceCollection RegisterApiModule(IServiceCollection builder) => builder;

    public abstract IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder);
}