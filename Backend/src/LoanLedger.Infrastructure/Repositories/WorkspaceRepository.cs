using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly LoanLedgerDbContext _context;

    public WorkspaceRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<Workspace?> GetByIdAsync(Guid id)
    {
        return await _context.Workspaces
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<Workspace>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Workspaces
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Workspace workspace)
    {
        await _context.Workspaces.AddAsync(workspace);
    }

    public void Update(Workspace workspace)
	{
		_context.Workspaces.Update(workspace);
	}

	public void Delete(Workspace workspace)
	{
		_context.Workspaces.Remove(workspace);
	}
}