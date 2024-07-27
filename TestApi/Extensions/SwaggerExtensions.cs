namespace TestApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger<T>(
        this IServiceCollection services,
        string title,
        string version)
        => services.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.UseInlineDefinitionsForEnums();
            c.CustomSchemaIds(x => x.FullName?.Replace("+", "_"));
        });

    public static IApplicationBuilder UseSwaggerForDevelopmentAndStaging(this WebApplication app)
    {
        if (!(app.Environment.IsDevelopment() || app.Environment.IsStaging()))
            return app;

        app.MapSwagger();
        app.UseSwaggerUI();

        app.MapGet("/", http =>
        {
            http.Response.Redirect("/swagger");

            return Task.CompletedTask;
        });

        return app;
    }
}