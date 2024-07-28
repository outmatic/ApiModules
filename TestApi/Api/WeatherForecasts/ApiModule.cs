using ApiModules.Abstractions;
using ApiModules.Extensions;

namespace TestApi.Api.WeatherForecasts;

public class ApiModule : BaseApiModule
{
    public override IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder.MapGroup("/weatherforecasts/{city}")
            .WithTags("WeatherForecasts");

        group.MapGetCommand<GetForecastsRequest>("/")
            .Produces<GetForecastsResponse>();

        return group;
    }
}