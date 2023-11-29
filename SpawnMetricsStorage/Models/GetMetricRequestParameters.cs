using System.ComponentModel.DataAnnotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;

namespace SpawnMetricsStorage.Models;

public class GetMetricRequestParameters
{
    [Required]
    [MinLength(ProjectNameConstants.MinProjectNameLength)]
    [MaxLength(ProjectNameConstants.MaxProjectNameLength)]
    public required string ProjectName { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.MinMetricNameLength)]
    [MaxLength(MetricRecordConstants.MaxMetricNameLength)]
    public required string MetricName { get; init; }
}