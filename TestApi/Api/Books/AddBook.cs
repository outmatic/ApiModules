using ApiModules.Abstractions;
using ApiModules.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data;

namespace TestApi.Api.Books;

public class AddBook(BookList books) : IApiCommand<AddBookRequest>
{
    public async Task<IResult> Handle(AddBookRequest addBookRequest, CancellationToken cancellationToken)
    {
        var id = books.Add(addBookRequest.Body.Title, addBookRequest.Body.Author);
        
        return Results.Ok(new AddBookResponse(id));
    }
}

public record AddBookRequest : IApiRequest
{
    [FromBody]
    public required RequestBody Body { get; init; }

    public record RequestBody(string Title, string Author);

    public class Validator : AbstractValidator<AddBookRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Body.Title).NotEmpty();
            RuleFor(x => x.Body.Author).NotEmpty();
        }
    }
}

public record AddBookResponse(Guid Id);