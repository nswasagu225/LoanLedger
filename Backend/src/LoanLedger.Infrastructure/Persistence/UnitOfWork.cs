using LoanLedger.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LoanLedger.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly LoanLedgerDbContext _context;

    private IDbContextTransaction? _transaction;

    public IUserRepository Users { get; }

    public IWorkspaceRepository Workspaces { get; }

    public UnitOfWork(
        LoanLedgerDbContext context,
        IUserRepository userRepository,
        IWorkspaceRepository workspaceRepository)
    {
        _context = context;

        Users = userRepository;
        Workspaces = workspaceRepository;
    }

    // =========================================================
    // BEGIN TRANSACTION
    // =========================================================

    public async Task BeginTransactionAsync()
    {
        _transaction =
            await _context.Database.BeginTransactionAsync();
    }

    // =========================================================
    // SAVE CHANGES
    // =========================================================

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // =========================================================
    // COMMIT TRANSACTION
    // =========================================================

    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.CommitAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    // =========================================================
    // ROLLBACK TRANSACTION
    // =========================================================

    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }
}