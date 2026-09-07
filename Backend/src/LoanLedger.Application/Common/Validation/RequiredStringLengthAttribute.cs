using System.ComponentModel.DataAnnotations;

namespace LoanLedger.Application.Common.Validation;

public sealed class RequiredStringLengthAttribute : ValidationAttribute
{
    private readonly int _minimumLength;
    private readonly int _maximumLength;

    public RequiredStringLengthAttribute(
        int minimumLength,
        int maximumLength)
    {
        _minimumLength = minimumLength;
        _maximumLength = maximumLength;
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not string text)
        {
            return new ValidationResult(
                "Invalid value.");
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            return ValidationResult.Success;
        }

        var trimmed = text.Trim();

        if (trimmed.Length < _minimumLength ||
            trimmed.Length > _maximumLength)
        {
            return new ValidationResult(
                ErrorMessage ??
                $"Value must be between {_minimumLength} and {_maximumLength} characters.");
        }

        return ValidationResult.Success;
    }
}