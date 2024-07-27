using ApiModules.Abstractions;
using ApiModules.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data;

namespace TestApi.Api.Books;

public class DeleteBook(BookList books) : IApiCommand<DeleteBookRequest>
{
    public async Task<IResult> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
    {
        var deleted = books.Delete(Guid.Parse(request.Id));

        return deleted switch
        {
            true => Results.Ok(),
            _ => Results.NotFound()
        };
    }
}

public record DeleteBookRequest : IApiRequest
{
    [FromRoute]
    public required string Id { get; init; }

    public record RequestBody(string Title, string Author);

    public class Validator : AbstractValidator<DeleteBookRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .Must(x => Guid.TryParse(x, out _))
                .WithMessage("Invalid Guid");
        }
    }
}

public record DeleteBookResponse(Guid Id);