using ApiModules.Abstractions;
using TestApi.Data;

namespace TestApi.Api.Books;

public class GetBooks(BookList books) : IApiCommand<GetBooks.Request>
{
    public record Request : IApiRequest;

    public record Response(int Count, List<Book> Books);

    public async Task<IResult> Handle(Request request, CancellationToken cancellationToken)
    {
        var books1 = books
            .List()
            .Select(x => new Book(x.Id, x.Title, x.Author))
            .ToList();
        
        return Results.Ok(new Response(books1.Count, books1));
    }
}