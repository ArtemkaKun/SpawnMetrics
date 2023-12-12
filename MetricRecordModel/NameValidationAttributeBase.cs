using System.ComponentModel.DataAnnotations;

namespace MetricRecordModel;

public abstract class NameValidationAttributeBase(int minLength, int maxLength) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            if (stringValue.Length < minLength || stringValue.Length > maxLength)
            {
                return new ValidationResult(GetErrorMessage(minLength, maxLength));
            }
        }
        else
        {
            return new ValidationResult("Invalid data type");
        }

        return ValidationResult.Success;
    }

    private static string GetErrorMessage(int minLength, int maxLength)
    {
        return $"The name must be at least {minLength} characters and at most {maxLength} characters long.";
    }
}