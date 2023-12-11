using System.ComponentModel.DataAnnotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace SpawnMetricsStorage.Models;

public sealed class GetMetricDataRangeRequestParameters
{
    private const string RangeStartPropertyName = nameof(RangeStart);
    private const string RangeEndPropertyName = nameof(RangeEnd);

    [Required]
    [IsValidProjectName]
    public required string ProjectName { get; init; }

    [Required]
    [IsValidMetricName]
    public required string MetricName { get; init; }

    [Required]
    [DateComparison(RangeEndPropertyName, DateComparisonRule.Earlier, ErrorMessage = $"{RangeStartPropertyName} must be earlier than {RangeEndPropertyName}")]
    public required DateTime RangeStart { get; init; }

    [Required]
    [DateComparison(RangeStartPropertyName, DateComparisonRule.Later, ErrorMessage = $"{RangeStartPropertyName} must be later than {RangeEndPropertyName}")]
    public required DateTime RangeEnd { get; init; }
}