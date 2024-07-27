using ApiModules.Abstractions;
using ApiModules.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestApi.Data;

namespace TestApi.Api.Books;

public class DeleteBook(BookList books) : IApiCommand<DeleteBook.Request>
{
    public record Request : IApiRequest
    {
        [FromRoute]
        public required string Id { get; init; }

        public record RequestBody(string Title, string Author);

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .Must(x => Guid.TryParse(x, out _))
                    .WithMessage("Invalid Guid");
            }
        }
    }

    public record Response(Guid Id);

    public async Task<IResult> Handle(Request request, CancellationToken cancellationToken)
    {
        var deleted = books.Delete(Guid.Parse(request.Id));

        return deleted switch
        {
            true => Results.Ok(),
            _ => Results.NotFound()
        };
    }
}