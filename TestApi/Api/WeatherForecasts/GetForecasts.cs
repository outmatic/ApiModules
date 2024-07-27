using ApiModules.Abstractions;

namespace TestApi.Api.WeatherForecasts;

public class GetForecasts : IApiCommand<GetForecastsRequest>
{
    public async Task<IResult> Handle(GetForecastsRequest request, CancellationToken cancellationToken)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        var forecasts = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        
        return TypedResults.Ok(new GetForecastsResponse(forecasts));
    }
}

public record GetForecastsResponse(WeatherForecast[] Forecasts);

public record GetForecastsRequest : IApiRequest;