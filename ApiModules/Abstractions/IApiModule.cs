namespace ApiModules.Abstractions;

public interface IApiModule
{
    IServiceCollection RegisterApiModule(IServiceCollection builder);
    IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder);
}