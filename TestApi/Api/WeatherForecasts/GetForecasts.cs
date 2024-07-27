using ApiModules.Abstractions;

namespace TestApi.Api.WeatherForecasts;

public class GetForecasts : IApiCommand<GetForecasts.Request>
{
    public record Request : IApiRequest;
   
    public record Response(string Message);

    public async Task<IResult> Handle(Request request, CancellationToken cancellationToken)
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
        
        return TypedResults.Ok(forecasts);
    }
}