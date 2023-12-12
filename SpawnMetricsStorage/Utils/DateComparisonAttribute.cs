using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Utils;

public sealed class DateComparisonAttribute(string dateTimePropertyToCompare, DateComparisonRule comparisonRule) : ValidationAttribute
{
    protected override ValidationResult IsValid(object? currentValueInObjectForm, ValidationContext validationContext)
    {
        if (currentValueInObjectForm == null)
            return new ValidationResult("Error: Current value is null.");

        var currentValue = (DateTime)currentValueInObjectForm;

        var comparisonPropertyInfo = validationContext.ObjectType.GetProperty(dateTimePropertyToCompare);

        if (comparisonPropertyInfo == null)
            return new ValidationResult($"Error: No property '{dateTimePropertyToCompare}' found.");

        var comparisonValueInObjectForm = comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);

        if (comparisonValueInObjectForm == null)
            return new ValidationResult("Error: Comparison value is null.");

        var comparisonValue = (DateTime)comparisonValueInObjectForm;

        switch (comparisonRule)
        {
            case DateComparisonRule.Earlier:
                if (currentValue >= comparisonValue)
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

                break;

            case DateComparisonRule.Later:
                if (currentValue <= comparisonValue)
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

                break;
        }

        return ValidationResult.Success!;
    }
}

public enum DateComparisonRule
{
    Earlier,
    Later
}