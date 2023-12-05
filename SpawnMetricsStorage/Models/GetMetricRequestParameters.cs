using System.ComponentModel.DataAnnotations;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace SpawnMetricsStorage.Models;

public class GetMetricRequestParameters
{
    [Required]
    [IsValidProjectName]
    public required string ProjectName { get; init; }

    [Required]
    [IsValidMetricName]
    public required string MetricName { get; init; }
}