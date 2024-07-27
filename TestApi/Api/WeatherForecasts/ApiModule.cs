using ApiModules.Abstractions;
using ApiModules.Extensions;

namespace TestApi.Api.WeatherForecasts;

public class ApiModule : BaseApiModule
{
    public override IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder.MapGroup("/weatherforecasts")
            .WithTags("WeatherForecasts");

        group.MapGetCommand<GetForecasts.Request>("/")
            .Produces<GetForecasts.Response>();

        return group;
    }
}