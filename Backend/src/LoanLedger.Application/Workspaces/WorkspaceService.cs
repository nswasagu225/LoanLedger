using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Workspaces;

public class WorkspaceService : IWorkspaceService
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkspaceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // =========================================================
    // CREATE WORKSPACE
    // =========================================================

    public async Task<WorkspaceResponse> CreateAsync(
        Guid userId,
        CreateWorkspaceRequest request)
    {
		if (request == null)
		throw new Exception("Workspace data is required.");

        ValidateName(request.Name);
		ValidateWorkspaceType(request.Type);

        var name = request.Name.Trim();

        var existingWorkspaces =
            await _unitOfWork.Workspaces.GetByUserIdAsync(userId);

        var duplicate =
            existingWorkspaces.Any(w =>
                !w.IsDeleted &&
                w.Name.Equals(
                    name,
                    StringComparison.OrdinalIgnoreCase));

        if (duplicate)
            throw new Exception(
                "A workspace with this name already exists.");

        var workspace = new Workspace
        {
            Id = Guid.NewGuid(),

            UserId = userId,

            Name = name,

            Type = request.Type,

            Description =
                string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim(),

            Currency =
                string.IsNullOrWhiteSpace(request.Currency)
                    ? null
                    : request.Currency.Trim().ToUpperInvariant(),

            IsActive = true,

            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Workspaces.AddAsync(workspace);

        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(workspace);
    }

    // =========================================================
    // GET CURRENT USER WORKSPACES
    // =========================================================

    public async Task<List<WorkspaceResponse>> GetAllAsync(
        Guid userId)
    {
        var workspaces =
            await _unitOfWork.Workspaces.GetByUserIdAsync(userId);

        return workspaces
            .Where(w => !w.IsDeleted)
            .OrderBy(w => w.Name)
            .Select(MapToResponse)
            .ToList();
    }

    // =========================================================
    // GET WORKSPACE BY ID
    // =========================================================

    public async Task<WorkspaceResponse?> GetByIdAsync(
        Guid userId,
        Guid workspaceId)
    {
        var workspace =
            await _unitOfWork.Workspaces.GetByIdAsync(workspaceId);

        if (workspace == null || workspace.IsDeleted)
            return null;

        EnsureOwnership(workspace, userId);

        return MapToResponse(workspace);
    }

    // =========================================================
    // UPDATE WORKSPACE
    // =========================================================

    public async Task<WorkspaceResponse?> UpdateAsync(
        Guid userId,
        Guid workspaceId,
        UpdateWorkspaceRequest request)
    {
		if (request == null)
		throw new Exception("Workspace data is required.");

        ValidateName(request.Name);

		ValidateWorkspaceType(request.Type);

        var workspace =
            await _unitOfWork.Workspaces.GetByIdAsync(workspaceId);

        if (workspace == null || workspace.IsDeleted)
            return null;

        EnsureOwnership(workspace, userId);

        var name = request.Name.Trim();

        var existingWorkspaces =
            await _unitOfWork.Workspaces.GetByUserIdAsync(userId);

        var duplicate =
            existingWorkspaces.Any(w =>
                w.Id != workspaceId &&
                !w.IsDeleted &&
                w.Name.Equals(
                    name,
                    StringComparison.OrdinalIgnoreCase));

        if (duplicate)
            throw new Exception(
                "A workspace with this name already exists.");

        workspace.Name = name;

        workspace.Type = request.Type;

        workspace.Description =
            string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();

        workspace.Currency =
            string.IsNullOrWhiteSpace(request.Currency)
                ? null
                : request.Currency.Trim().ToUpperInvariant();

        workspace.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Workspaces.Update(workspace);

        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(workspace);
    }

    // =========================================================
    // ACTIVATE WORKSPACE
    // =========================================================

    public async Task<bool> ActivateAsync(
        Guid userId,
        Guid workspaceId)
    {
        var workspace =
            await _unitOfWork.Workspaces.GetByIdAsync(workspaceId);

        if (workspace == null || workspace.IsDeleted)
            return false;

        EnsureOwnership(workspace, userId);

        if (workspace.IsActive)
            return true;

        workspace.IsActive = true;

        workspace.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Workspaces.Update(workspace);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    // =========================================================
    // DEACTIVATE WORKSPACE
    // =========================================================

    public async Task<bool> DeactivateAsync(
        Guid userId,
        Guid workspaceId)
    {
        var workspace =
            await _unitOfWork.Workspaces.GetByIdAsync(workspaceId);

        if (workspace == null || workspace.IsDeleted)
            return false;

        EnsureOwnership(workspace, userId);

        if (!workspace.IsActive)
            return true;

        workspace.IsActive = false;

        workspace.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Workspaces.Update(workspace);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    // =========================================================
    // SOFT DELETE WORKSPACE
    // =========================================================

    public async Task<bool> DeleteAsync(
        Guid userId,
        Guid workspaceId)
    {
        var workspace =
            await _unitOfWork.Workspaces.GetByIdAsync(workspaceId);

        if (workspace == null || workspace.IsDeleted)
            return false;

        EnsureOwnership(workspace, userId);

        workspace.IsDeleted = true;

        workspace.IsActive = false;

        workspace.DeletedAt = DateTime.UtcNow;

        workspace.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Workspaces.Update(workspace);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    // =========================================================
    // VALIDATION
    // =========================================================

    private static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception(
                "Workspace name is required.");

        if (name.Trim().Length > 150)
            throw new Exception(
                "Workspace name cannot exceed 150 characters.");
    }
	private static void ValidateWorkspaceType(WorkspaceType type)
	{
		if (!Enum.IsDefined(typeof(WorkspaceType), type))
		{
			throw new Exception(
				"Invalid workspace type. " +
				"Allowed values are Individual, Business, Cooperative, or Organization.");
		}
	}
    // =========================================================
    // OWNERSHIP
    // =========================================================

    private static void EnsureOwnership(
        Workspace workspace,
        Guid userId)
    {
        if (workspace.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "You do not have access to this workspace.");
        }
    }

    // =========================================================
    // MAPPER
    // =========================================================

    private static WorkspaceResponse MapToResponse(
        Workspace workspace)
    {
        return new WorkspaceResponse
        {
            Id = workspace.Id,

            UserId = workspace.UserId,

            Name = workspace.Name,

            Type = workspace.Type,

            TypeName = workspace.Type.ToString(),

            Description = workspace.Description,

            Currency = workspace.Currency,

            IsActive = workspace.IsActive,

            CreatedAt = workspace.CreatedAt
        };
    }
}