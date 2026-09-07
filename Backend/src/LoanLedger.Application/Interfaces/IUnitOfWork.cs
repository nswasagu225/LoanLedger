namespace LoanLedger.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IWorkspaceRepository Workspaces { get; }

    Task BeginTransactionAsync();

    Task SaveChangesAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}