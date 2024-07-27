using ApiModules.Abstractions;
using TestApi.Data;

namespace TestApi.Api.Books;

public class GetBooks(BookList books) : IApiCommand<GetBooksRequest>
{
    public async Task<IResult> Handle(GetBooksRequest request, CancellationToken cancellationToken)
    {
        var books1 = books
            .List()
            .Select(x => new Book(x.Id, x.Title, x.Author))
            .ToList();
        
        return Results.Ok(new GetBooksResponse(books1.Count, books1));
    }
}

public record GetBooksRequest : IApiRequest;

public record GetBooksResponse(int Count, List<Book> Books);