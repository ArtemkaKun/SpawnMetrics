using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MetricRecordModel;
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
    public string branchName { get; } = branchName;

    [Required]
    [MinLength(3)]
    public string remoteName { get; } = remoteName;

    [Required]
    [IsValidProjectName]
    public string projectName { get; } = projectName;

    [Required]
    [MinLength(MetricRecordConstants.MinCommitGitHubUrlLength, ErrorMessage = MetricRecordConstants.CommitGitHubUrlShorterErrorMessage)]
    [MaxLength(MetricRecordConstants.MaxCommitGitHubUrlLength)]
    [Url]
    [RegularExpression(@"https:\/\/github\.com\/[^\/]+\/[^\/]+\/commit\/", ErrorMessage = "Invalid base GitHub commit URL")]
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
    public string name { get; } = name;

    [Required]
    [MinLength(3)]
    [MaxLength(2048)]
    public string command { get; } = command;

    [Required]
    [IsValidUnits]
    public string units { get; } = units;
}