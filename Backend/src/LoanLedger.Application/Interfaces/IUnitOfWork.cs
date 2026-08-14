namespace LoanLedger.Application.Interfaces;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();

    Task SaveChangesAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}