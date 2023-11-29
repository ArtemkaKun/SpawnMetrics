using System.ComponentModel.DataAnnotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;

namespace SpawnMetricsStorage.Models;

public sealed class LogMetricRequestBody
{
    [Required]
    [MinLength(ProjectNameConstants.MinProjectNameLength)]
    [MaxLength(ProjectNameConstants.MaxProjectNameLength)]
    public required string ProjectName { get; init; }

    [Required]
    public required MetricRecord Metric { get; init; }
}