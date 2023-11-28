using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models;

public sealed class MetricRecord
{
    [Required, StringLength(100, ErrorMessage = "Metric name cannot be longer than 100 characters.")]
    public required string Name { get; init; }

    [Required] public required DateTime LogTime { get; init; }

    [Required, RegularExpression(@"^[\da-fA-F]{7,40}$", ErrorMessage = "Invalid short commit hash.")]
    public required string ShortCommitHash { get; init; }

    [Required, Url] public required string CommitLink { get; init; }

    [Required, StringLength(500, ErrorMessage = "Commit message cannot be longer than 500 characters.")]
    public required string CommitMessage { get; init; }

    [Required, MinLength(1)] public required string Value { get; init; }

    [Required, MinLength(1)] public required string Units { get; init; }
}