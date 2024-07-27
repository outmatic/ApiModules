using ApiModules.Abstractions;
using ApiModules.Extensions;

namespace TestApi.Api.Books;

public class ApiModule : BaseApiModule
{
    public override IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder.MapGroup("/books")
            .WithTags("Books");

        group.MapGetCommand<GetBooks.Request>("/")
            .Produces<GetBooks.Response>();

        group.MapPostCommand<AddBook.Request>("/")
            .Produces<AddBook.Response>();
        
        group.MapDeleteCommand<DeleteBook.Request>("/{id}")
            .Produces<DeleteBook.Response>();

        return group;
    }
}