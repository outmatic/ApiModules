using ApiModules.Extensions;
using TestApi.Data;
using TestApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookList>();
builder.Services.AddApiEndpoints<Program>()
    .AddSwagger<Program>("Test Api", "1.0");

var app = builder.Build();

app.UseSwaggerForDevelopmentAndStaging();
app.UseApiEndpoints();
app.Run();