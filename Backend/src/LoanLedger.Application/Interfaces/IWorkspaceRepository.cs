using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IWorkspaceRepository
{
    Task<Workspace?> GetByIdAsync(Guid id);

    Task<IEnumerable<Workspace>> GetByUserIdAsync(Guid userId);

    Task AddAsync(Workspace workspace);

    void Update(Workspace workspace);

	void Delete(Workspace workspace);
}