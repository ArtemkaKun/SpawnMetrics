using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models;

public sealed class GetMetricDataRangeRequestParameters : GetMetricRequestParameters
{
    private const string RangeStartPropertyName = nameof(RangeStart);
    private const string RangeEndPropertyName = nameof(RangeEnd);

    [Required]
    [DateComparison(RangeEndPropertyName, DateComparisonRule.Earlier, ErrorMessage = $"{RangeStartPropertyName} must be earlier than {RangeEndPropertyName}")]
    public required DateTime RangeStart { get; init; }

    [Required]
    [DateComparison(RangeStartPropertyName, DateComparisonRule.Later, ErrorMessage = $"{RangeStartPropertyName} must be later than {RangeEndPropertyName}")]
    public required DateTime RangeEnd { get; init; }
}