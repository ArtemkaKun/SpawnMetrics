using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models;

public sealed class MetricRecord
{
    [Required] public required string Name { get; init; }

    [Required] public required string Value { get; init; }

    [Required] public required string Units { get; init; }

    [Required] public required DateTime LogTime { get; init; }
}