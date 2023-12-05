using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models.MetricRecordFiles;

public sealed class IsValidMetricNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            if (stringValue.Length is < MetricRecordConstants.MinMetricNameLength or > MetricRecordConstants.MaxMetricNameLength)
            {
                return new ValidationResult(GetErrorMessage(MetricRecordConstants.MinMetricNameLength, MetricRecordConstants.MaxMetricNameLength));
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