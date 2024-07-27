using MediatR;

namespace ApiModules.Abstractions;

public interface IApiCommand<in TIn> : IRequestHandler<TIn, IResult>
    where TIn : IApiRequest
{
}