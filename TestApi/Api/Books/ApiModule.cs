using ApiModules.Abstractions;
using ApiModules.Extensions;

namespace TestApi.Api.Books;

public class ApiModule : BaseApiModule
{
    public override IEndpointRouteBuilder MapApiModule(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder.MapGroup("/books")
            .WithTags("Books");

        group.MapGetCommand<GetBooksRequest>("/")
            .Produces<GetBooksResponse>();

        group.MapPostCommand<AddBookRequest>("/")
            .Produces<AddBookResponse>();
        
        group.MapDeleteCommand<DeleteBookRequest>("/{id}")
            .Produces<DeleteBookResponse>();

        return group;
    }
}