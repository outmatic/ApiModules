using System.Reflection;
using ApiModules.Transactions.Abstractions;
using MediatR;

namespace ApiModules.Behaviours;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
    private readonly ITransactionContext _transactionContext;

    public TransactionBehavior(
        IRequestHandler<TRequest, TResponse> requestHandler,
        ITransactionContext transactionContext)
    {
        _requestHandler = requestHandler;
        _transactionContext = transactionContext;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var attr = _requestHandler.GetType().GetCustomAttribute<TransactionalAttribute>();
        if (attr is null)
            return await next();

        await _transactionContext.StartTransactionAsync(cancellationToken);
        try
        {
            var result = await next();
            await _transactionContext.CommitTransactionAsync(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            await _transactionContext.RollbackTransactionAsync(cancellationToken);
            
            throw;
        }
    }
}