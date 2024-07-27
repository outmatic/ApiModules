using ApiModules.Transactions.Abstractions;
using MongoDB.Driver;

namespace ApiModules.MongoDb.Transactions;

public class MongoDbTransactionContext : ITransactionContext
{
    private readonly IClientSessionHandle _clientSession;

    public MongoDbTransactionContext(IClientSessionHandle clientSession)
        => _clientSession = clientSession;

    public Task StartTransactionAsync(CancellationToken cancellationToken)
    {
        _clientSession.StartTransaction();

        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken)
        => _clientSession.IsInTransaction switch
        {
            true => _clientSession.CommitTransactionAsync(cancellationToken),
            _ => Task.CompletedTask
        };

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
        => _clientSession.IsInTransaction switch
        {
            true => _clientSession.AbortTransactionAsync(cancellationToken),
            _ => Task.CompletedTask
        };
}