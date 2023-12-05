using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models.ProjectName;

public sealed class IsValidProjectName : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            if (stringValue.Length is < ProjectNameConstants.MinProjectNameLength or > ProjectNameConstants.MaxProjectNameLength)
            {
                return new ValidationResult(GetErrorMessage(ProjectNameConstants.MinProjectNameLength, ProjectNameConstants.MaxProjectNameLength));
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
        return $"The metric name must be at least {minLength} characters and at most {maxLength} characters long.";
    }
}