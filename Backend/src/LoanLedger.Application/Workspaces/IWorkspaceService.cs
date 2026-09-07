namespace LoanLedger.Application.Workspaces;

public interface IWorkspaceService
{
    Task<WorkspaceResponse> CreateAsync(
        Guid userId,
        CreateWorkspaceRequest request);

    Task<List<WorkspaceResponse>> GetAllAsync(
        Guid userId);

    Task<WorkspaceResponse?> GetByIdAsync(
        Guid userId,
        Guid workspaceId);

    Task<WorkspaceResponse?> UpdateAsync(
        Guid userId,
        Guid workspaceId,
        UpdateWorkspaceRequest request);

    Task<bool> ActivateAsync(
        Guid userId,
        Guid workspaceId);

    Task<bool> DeactivateAsync(
        Guid userId,
        Guid workspaceId);

    Task<bool> DeleteAsync(
        Guid userId,
        Guid workspaceId);
}