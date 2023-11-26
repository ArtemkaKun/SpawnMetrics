using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models;

public sealed class MetricRecord<TValue>
{
    [Required]
    public required string Name { get; init; }
    
    [Required]
    public required TValue Value { get; init; }
    
    [Required]
    public required string Units { get; init; }
    
    [Required]
    public required DateTime LogTime { get; init; }
}