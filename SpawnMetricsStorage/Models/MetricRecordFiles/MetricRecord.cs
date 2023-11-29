using System.ComponentModel.DataAnnotations;

namespace SpawnMetricsStorage.Models.MetricRecordFiles;

public sealed class MetricRecord
{
    [Required]
    // TODO: Combine into one attribute since these are used in a few places
    [MinLength(MetricRecordConstants.MinMetricNameLength)]
    [MaxLength(MetricRecordConstants.MaxMetricNameLength)]
    public required string Name { get; init; }

    [Required]
    public required DateTime LogTime { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.ShortCommitHashLength)]
    [MaxLength(MetricRecordConstants.ShortCommitHashLength)]
    [RegularExpression(@"^[\da-fA-F]{7,8}$", ErrorMessage = "Invalid short commit hash")]
    public required string ShortCommitHash { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.CommitGitHubUrlLength, ErrorMessage = MetricRecordConstants.CommitGitHubUrlShorterErrorMessage)]
    [MaxLength(MetricRecordConstants.CommitGitHubUrlLength, ErrorMessage = MetricRecordConstants.CommitGitHubUrlLongerErrorMessage)]
    [Url]
    public required string CommitGitHubUrl { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    [MaxLength(MetricRecordConstants.MaxCommitMessageLength)]
    public required string CommitMessage { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    public required string Value { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    [MaxLength(MetricRecordConstants.MaxUnitsLength)]
    public required string Units { get; init; }
}