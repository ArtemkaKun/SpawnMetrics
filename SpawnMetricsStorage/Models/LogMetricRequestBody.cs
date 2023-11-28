using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models;

public sealed class LogMetricRequestBody
{
    [Required, StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string ProjectName { get; init; }

    [Required]
    public required MetricRecord Metric { get; init; }
}