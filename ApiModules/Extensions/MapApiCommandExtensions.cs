using ApiModules.Abstractions;
using ApiModules.Validation;
using MediatR;

namespace ApiModules.Extensions;

public static class MapApiCommandExtensions
{
    private static async Task<IResult?> Mediator<TRequest>(
        [AsParameters, Validate] TRequest request,
        IMediator mediator,
        CancellationToken cancellationToken) where TRequest : IApiRequest
        => await mediator.Send(request, cancellationToken) switch
        {
            { } result => result,
            _ => throw new Exception("Unknown response type")
        };

    public static RouteHandlerBuilder MapGetCommand<TRequest>(
        this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
        where TRequest : class, IApiRequest
        => endpointRouteBuilder.MapGet(pattern, Mediator<TRequest>)
            .Produces(StatusCodes.Status400BadRequest);

    public static RouteHandlerBuilder MapPostCommand<TRequest>(
        this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
        where TRequest : class, IApiRequest
        => endpointRouteBuilder.MapPost(pattern, Mediator<TRequest>)
            .Produces(StatusCodes.Status400BadRequest);

    public static RouteHandlerBuilder MapPutCommand<TRequest>(
        this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
        where TRequest : class, IApiRequest
        => endpointRouteBuilder.MapPut(pattern, Mediator<TRequest>)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

    public static RouteHandlerBuilder MapPatchCommand<TRequest>(
        this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
        where TRequest : class, IApiRequest
        => endpointRouteBuilder.MapPatch(pattern, Mediator<TRequest>)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

    public static RouteHandlerBuilder MapDeleteCommand<TRequest>(
        this IEndpointRouteBuilder endpointRouteBuilder,
        string pattern)
        where TRequest : class, IApiRequest
        => endpointRouteBuilder.MapDelete(pattern, Mediator<TRequest>)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent);
}