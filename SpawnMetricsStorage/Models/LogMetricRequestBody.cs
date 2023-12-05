using System.ComponentModel.DataAnnotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;

namespace SpawnMetricsStorage.Models;

// NOTE: This class can be readonly structure instead of class, which will be more convenient dues to the logic.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support validation of value types.
public sealed class LogMetricRequestBody
{
    [Required]
    [MinLength(ProjectNameConstants.MinProjectNameLength)]
    [MaxLength(ProjectNameConstants.MaxProjectNameLength)]
    public required string ProjectName { get; init; }

    [Required]
    public required MetricRecord Metric { get; init; }
}