using ApiModules.Abstractions;
using ApiModules.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data;

namespace TestApi.Api.Books;

public class AddBook(BookList books) : IApiCommand<AddBook.Request>
{
    public record Request : IApiRequest
    {
        [FromBody]
        public required RequestBody Body { get; init; }

        public record RequestBody(string Title, string Author);

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Body.Title).NotEmpty();
                RuleFor(x => x.Body.Author).NotEmpty();
            }
        }
    }

    public record Response(Guid Id);

    public async Task<IResult> Handle(Request request, CancellationToken cancellationToken)
    {
        var id = books.Add(request.Body.Title, request.Body.Author);
        
        return Results.Ok(new Response(id));
    }
}