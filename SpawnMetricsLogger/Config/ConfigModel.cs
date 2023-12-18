using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MetricRecordModel;
using SharedConstants;
using SpawnMetricsStorage.Models.ProjectName;

namespace SpawnMetricsLogger.Config;

// NOTE: This class can be readonly structure instead of class, which will be more convenient dues to the logic.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support validation of value types.
[method: JsonConstructor]
public sealed class ConfigModel(string dataServerUrl, string branchName, string remoteName, string projectName, string baseCommitGitHubUrl, List<MetricOperationModel> metricOperations)
{
    [Required]
    [MinLength(21)]
    [Url]
    public string dataServerUrl { get; } = dataServerUrl;

    [Required]
    [MinLength(3)]
    [MaxLength(244)]
    [RegularExpression(@"^(?!\/|\.|\-|.*[\x00-\x1F\x7F]|.*[~^:?*\[\]\\ ]|.*@{|\.\.|\@|.*\/{2,}|.*\/$|.*\.lock$|.*\.\.)[a-zA-Z0-9\/_\-\.]+$", ErrorMessage = "Invalid branch name")]
    public string branchName { get; } = branchName;

    [Required]
    [MinLength(3)]
    [MaxLength(244)]
    [RegularExpression(@"^(?!\/|\.|\-|.*[\x00-\x1F\x7F]|.*[~^:?*\[\]\\ ]|.*@{|\.\.|\@|.*\/{2,}|.*\/$|.*\.lock$|.*\.\.)[a-zA-Z0-9\/_\-\.]+$", ErrorMessage = "Invalid remote name")]
    public string remoteName { get; } = remoteName;

    [Required]
    [IsValidProjectName]
    public string projectName { get; } = projectName;

    [Required]
    [MinLength(GitConstants.MinBaseCommitGitHubUrlLength, ErrorMessage = GitConstants.BaseCommitGitHubUrlShorterErrorMessage)]
    [MaxLength(GitConstants.MaxCommitGitHubUrlLength)]
    [Url]
    [RegularExpression(GitConstants.BaseGitHubUrlCheckRegex, ErrorMessage = "Invalid base GitHub commit URL")]
    public string baseCommitGitHubUrl { get; } = baseCommitGitHubUrl;

    [Required]
    [MinLength(1)]
    public List<MetricOperationModel> metricOperations { get; } = metricOperations;
}

// NOTE: This structure can have readonly fields instead of readonly properties, which will be more convenient and easier to read.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support DataAnnotations attributes for fields.
[method: JsonConstructor]
public readonly struct MetricOperationModel(string name, string command, string units)
{
    [Required]
    [IsValidMetricName]
    public string Name { get; } = name;

    [Required]
    [MinLength(3)]
    [MaxLength(2048)]
    public string Command { get; } = command;

    [Required]
    [IsValidUnits]
    public string Units { get; } = units;
}