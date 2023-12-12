using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;
using SpawnMetricsStorage.Utils;

namespace SpawnMetricsStorage.Models;

// NOTE: This class can be readonly structure instead of class, which will be more convenient dues to the logic.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support validation of value types.
[UsedImplicitly]
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