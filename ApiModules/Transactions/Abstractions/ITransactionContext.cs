namespace ApiModules.Transactions.Abstractions;

public interface ITransactionContext
{
    Task StartTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}