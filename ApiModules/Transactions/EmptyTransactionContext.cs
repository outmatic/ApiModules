using ApiModules.Transactions.Abstractions;

namespace ApiModules.Transactions;

public class EmptyTransactionContext : ITransactionContext
{
    public Task StartTransactionAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task CommitTransactionAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task RollbackTransactionAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}