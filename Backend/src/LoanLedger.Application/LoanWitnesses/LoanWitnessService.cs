using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.LoanWitnesses;

public class LoanWitnessService : ILoanWitnessService
{
    private const int MaximumWitnessesPerLoan = 3;

    private readonly ILoanWitnessRepository _repository;
    private readonly ILoanRepository _loanRepository;
    private readonly IWitnessRepository _witnessRepository;

    public LoanWitnessService(
        ILoanWitnessRepository repository,
        ILoanRepository loanRepository,
        IWitnessRepository witnessRepository)
    {
        _repository = repository;
        _loanRepository = loanRepository;
        _witnessRepository = witnessRepository;
    }

    public async Task<LoanWitnessResponse> AddAsync(
        Guid userId,
        Guid loanId,
        AddLoanWitnessRequest request)
    {
        // ---------------------------------------------------------
        // VALIDATE LOAN
        // ---------------------------------------------------------

        var loan =
            await _loanRepository.GetByIdAsync(loanId);

        if (loan == null ||
            loan.UserId != userId)
        {
            throw new InvalidOperationException(
                "Loan not found or does not belong to the current user.");
        }

        // ---------------------------------------------------------
        // VALIDATE WITNESS
        // ---------------------------------------------------------

        var witness =
            await _witnessRepository.GetByIdAsync(
                request.WitnessId);

        if (witness == null ||
            witness.UserId != userId)
        {
            throw new InvalidOperationException(
                "Witness not found or does not belong to the current user.");
        }

        // ---------------------------------------------------------
        // PREVENT DUPLICATE
        // ---------------------------------------------------------

        var alreadyExists =
            await _repository.ExistsAsync(
                loanId,
                request.WitnessId);

        if (alreadyExists)
        {
            throw new InvalidOperationException(
                "This witness is already attached to the loan.");
        }

        // ---------------------------------------------------------
        // MAXIMUM THREE WITNESSES
        // ---------------------------------------------------------

        var currentCount =
            await _repository.CountByLoanAsync(
                loanId);

        if (currentCount >= MaximumWitnessesPerLoan)
        {
            throw new InvalidOperationException(
                "A loan can have a maximum of 3 witnesses.");
        }

        // ---------------------------------------------------------
        // DETERMINE ORDER
        // ---------------------------------------------------------

        var existing =
            await _repository.GetByLoanAsync(
                loanId);

        int order;

        if (request.WitnessOrder.HasValue)
        {
            order = request.WitnessOrder.Value;

            if (order < 1 ||
                order > MaximumWitnessesPerLoan)
            {
                throw new ArgumentException(
                    "Witness order must be between 1 and 3.");
            }

            if (existing.Any(x =>
                x.WitnessOrder == order))
            {
                throw new InvalidOperationException(
                    $"Witness order {order} is already in use.");
            }
        }
        else
        {
            order = Enumerable
                .Range(1, MaximumWitnessesPerLoan)
                .First(x =>
                    !existing.Any(w =>
                        w.WitnessOrder == x));
        }

        // ---------------------------------------------------------
        // CREATE RELATIONSHIP
        // ---------------------------------------------------------

        var loanWitness = new LoanWitness
        {
            LoanId = loanId,
            WitnessId = request.WitnessId,
            WitnessOrder = order,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(
            loanWitness);

        await _repository.SaveChangesAsync();

        // Attach witness data for response
        loanWitness.Witness = witness;

        return Map(loanWitness);
    }

    public async Task<List<LoanWitnessResponse>> GetByLoanAsync(
        Guid userId,
        Guid loanId)
    {
        var loan =
            await _loanRepository.GetByIdAsync(
                loanId);

        if (loan == null ||
            loan.UserId != userId)
        {
            throw new InvalidOperationException(
                "Loan not found or does not belong to the current user.");
        }

        var witnesses =
            await _repository.GetByLoanAsync(
                loanId);

        return witnesses
            .Select(Map)
            .ToList();
    }

    public async Task<bool> RemoveAsync(
        Guid userId,
        Guid loanId,
        Guid witnessId)
    {
        var loan =
            await _loanRepository.GetByIdAsync(
                loanId);

        if (loan == null ||
            loan.UserId != userId)
        {
            return false;
        }

        var loanWitness =
            await _repository.GetByLoanAndWitnessAsync(
                loanId,
                witnessId);

        if (loanWitness == null)
        {
            return false;
        }

        await _repository.DeleteAsync(
            loanWitness);

        await _repository.SaveChangesAsync();

        return true;
    }

    private static LoanWitnessResponse Map(
        LoanWitness loanWitness)
    {
        return new LoanWitnessResponse
        {
            Id = loanWitness.Id,
            LoanId = loanWitness.LoanId,
            WitnessId = loanWitness.WitnessId,
            WitnessOrder = loanWitness.WitnessOrder,
            CreatedAt = loanWitness.CreatedAt,

            FullName =
                loanWitness.Witness.FullName,

            PhoneNumber =
                loanWitness.Witness.PhoneNumber,

            Email =
                loanWitness.Witness.Email,

            Address =
                loanWitness.Witness.Address,

            Relationship =
                loanWitness.Witness.Relationship,

            IdentificationType =
                loanWitness.Witness.IdentificationType,

            IdentificationNumber =
                loanWitness.Witness.IdentificationNumber,

            Notes =
                loanWitness.Witness.Notes
        };
    }
}