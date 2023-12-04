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
    public required DateTime LogTimeUtc { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.ShortCommitHashLength)]
    [MaxLength(MetricRecordConstants.ShortCommitHashLength)]
    [RegularExpression(@"^[\da-fA-F]{8}$", ErrorMessage = "Invalid short commit hash")]
    public required string ShortCommitHash { get; init; }

    [Required]
    [MinLength(MetricRecordConstants.MinCommitGitHubUrlLength, ErrorMessage = MetricRecordConstants.CommitGitHubUrlShorterErrorMessage)]
    [MaxLength(MetricRecordConstants.MaxCommitGitHubUrlLength)]
    [Url]
    [RegularExpression(@"https:\/\/github\.com\/[^\/]+\/[^\/]+\/commit\/[\da-fA-F]{8}", ErrorMessage = "Invalid GitHub commit URL")]
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