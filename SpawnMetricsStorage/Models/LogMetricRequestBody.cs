using System.ComponentModel.DataAnnotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace SpawnMetricsStorage.Models;

// NOTE: This class can be readonly structure instead of class, which will be more convenient dues to the logic.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support validation of value types.
public sealed class LogMetricRequestBody
{
    [Required]
    [IsValidProjectName]
    public required string ProjectName { get; init; }

    [Required]
    public required MetricRecord Metric { get; init; }
}