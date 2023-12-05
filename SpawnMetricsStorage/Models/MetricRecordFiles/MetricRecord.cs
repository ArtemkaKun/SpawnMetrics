using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SpawnMetricsStorage.Models.MetricRecordFiles;

// NOTE: This structure can have readonly fields instead of readonly properties, which will be more convenient and easier to read.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support DataAnnotations attributes for fields.
[method: JsonConstructor]
public readonly struct MetricRecord(string name, DateTime logTimeUtc, string commitGitHubUrl, string commitMessage, string value, string units)
{
    [Required]
    [IsValidMetricName]
    public string Name { get; } = name;

    [Required]
    public DateTime LogTimeUtc { get; } = logTimeUtc;

    [Required]
    [MinLength(MetricRecordConstants.MinCommitGitHubUrlLength, ErrorMessage = MetricRecordConstants.CommitGitHubUrlShorterErrorMessage)]
    [MaxLength(MetricRecordConstants.MaxCommitGitHubUrlLength)]
    [Url]
    [RegularExpression(@"https:\/\/github\.com\/[^\/]+\/[^\/]+\/commit\/[\da-fA-F]{8}", ErrorMessage = "Invalid GitHub commit URL")]
    public string CommitGitHubUrl { get; } = commitGitHubUrl;

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    [MaxLength(MetricRecordConstants.MaxCommitMessageLength)]
    public string CommitMessage { get; } = commitMessage;

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    public string Value { get; } = value;

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    [MaxLength(MetricRecordConstants.MaxUnitsLength)]
    public string Units { get; } = units;
}