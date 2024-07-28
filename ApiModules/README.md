# ApiModules

ApiModules is a .NET library that enhances Minimal APIs by simplifying route organization and integrating with MediatR and FluentValidation. It also offers automatic transaction handling.

## Why ApiModules?

We developed ApiModules while building a fintech platform to address two main issues in our microservices:

1. Excessive boilerplate code, especially in controllers
2. Complex mappings between requests, commands/queries, and responses

By adopting the REPR (Request-Endpoint-Response) pattern and Minimal APIs, ApiModules streamlines development and ensures consistency across our team.

## Project Status

ApiModules is currently a work in progress, but it's already being used successfully in production environments in high-stakes scenarios. While we continue to refine and expand the library and its documentation, you can confidently use ApiModules in your projects.

## Quick Start

1. Create a new web application:
   ```
   dotnet new web --output ExampleApi
   ```

2. Add the ApiModules package:
   ```
   dotnet add package ApiModules --version 1.0.0-beta
   ```

3. Update your `Program.cs`:
   ```csharp
   using ApiModules;

   var builder = WebApplication.CreateBuilder(args);
   builder.Services.AddApiEndpoints<Program>();

   var app = builder.Build();
   app.UseApiEndpoints();
   app.Run();
   ```

## Creating Your First Endpoint

1. Define request and response objects `GetForecasts.cs`:

   ```csharp
   public record GetForecastsRequest([FromRoute] string City) : IApiRequest;
   public record GetWeatherResponse(WeatherForecast[] Forecasts);
   ```

2. Create a handler `GetForecasts.cs`:

   ```csharp
   public class GetForecasts : IApiCommand<GetForecastsRequest>
   {
       public async Task<IResult> Handle(GetForecastsRequest request, CancellationToken cancellationToken)
       {
            if (request.City.Equals("Gotham City", StringComparison.OrdinalIgnoreCase))
                return TypeResults.NotFound(); 
           
           // More handler logic to retrieve forecasts
           
           return TypedResults.Ok(new GetWeatherResponse(forecasts));
       }
   }
   ```

3. Define an ApiModule to map routes:

   ```csharp
   public class ApiModule : BaseApiModule
   {
       public override IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder)
       {
           var group = endpointRouteBuilder.MapGroup("/weatherforecasts")
               .WithTags("WeatherForecasts");

           group.MapGetCommand<GetForecastsRequest>("/{city}")
               .Produces<GetForecastsResponse>()
               .Produces<NotFound>();

           return group;
       }
   }
   ```

ApiModules automatically scans for classes implementing `BaseApiModule` and registers all routes during startup.

## Further Examples

To explore implementations of other HTTP verbs and see more usage scenarios, check out our examples in the TestApi directory of our GitHub repository:

[ApiModules TestApi Examples](https://github.com/outmatic/ApiModules/tree/main/TestApi)

These examples demonstrate how to leverage ApiModules for various HTTP methods and showcase best practices for structuring your API endpoints.
