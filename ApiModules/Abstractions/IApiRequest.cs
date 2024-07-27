using MediatR;

namespace ApiModules.Abstractions;

public interface IApiRequest : IRequest<IResult>
{
}