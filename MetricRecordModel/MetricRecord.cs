using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using SharedConstants;

namespace MetricRecordModel;

// NOTE: This structure can have readonly fields instead of readonly properties, which will be more convenient and easier to read.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support DataAnnotations attributes for fields.
[method: JsonConstructor]
public readonly struct MetricRecord(string name, DateTime logTimeUtc, string commitGitHubUrl, string commitMessage, string value, string units)
{
    [Required]
    [IsValidMetricName]
    // NOTE: Used as structure for request/DB data, so this field serialized by JSON serializers.
    [UsedImplicitly]
    public string Name { get; } = name;

    [Required]
    public DateTime LogTimeUtc { get; } = logTimeUtc;

    [Required]
    [MinLength(GitConstants.MinCommitGitHubUrlLength, ErrorMessage = GitConstants.CommitGitHubUrlShorterErrorMessage)]
    [MaxLength(GitConstants.MaxCommitGitHubUrlLength)]
    [Url]
    [RegularExpression(GitConstants.GitHubUrlCheckRegex, ErrorMessage = "Invalid GitHub commit URL")]
    // NOTE: Used as structure for request/DB data, so this field serialized by JSON serializers.
    [UsedImplicitly]
    public string CommitGitHubUrl { get; } = commitGitHubUrl;

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    [MaxLength(MetricRecordConstants.MaxCommitMessageLength)]
    // NOTE: Used as structure for request/DB data, so this field serialized by JSON serializers.
    [UsedImplicitly]
    public string CommitMessage { get; } = commitMessage;

    [Required]
    [MinLength(MetricRecordConstants.MinStringLength)]
    // NOTE: Used as structure for request/DB data, so this field serialized by JSON serializers.
    [UsedImplicitly]
    public string Value { get; } = value;

    [Required]
    [IsValidUnits]
    // NOTE: Used as structure for request/DB data, so this field serialized by JSON serializers.
    [UsedImplicitly]
    public string Units { get; } = units;
}