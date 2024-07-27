using ApiModules.Extensions;
using TestApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookList>();
builder.Services.AddApiEndpoints<Program>();

var app = builder.Build();

app.UseApiEndpoints();
app.Run();