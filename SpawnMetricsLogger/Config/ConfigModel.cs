using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MetricRecordModel;
using SharedConstants;
using SpawnMetricsStorage.Models.ProjectName;

namespace SpawnMetricsLogger.Config;

// NOTE: This class can be readonly structure instead of class, which will be more convenient dues to the logic.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support validation of value types.
[method: JsonConstructor]
public sealed class ConfigModel(string dataServerUrl, string branchName, string projectName, string baseCommitGitHubUrl, List<MetricOperationModel> metricOperations)
{
    private const string GitBranchValidationRegex = @"^(?!\/|\.|\-|.*[\x00-\x1F\x7F]|.*[~^:?*\[\]\\ ]|.*@{|\.\.|\@|.*\/{2,}|.*\/$|.*\.lock$|.*\.\.)[a-zA-Z0-9\/_\-\.]+$";
    private const int MinGitBranchNameLength = 3;
    private const int MaxGitBranchNameLength = 244; // NOTE: this is a limit of GitHub

    [Required]
    [MinLength(16, ErrorMessage = "Data server URL can't be shorter than 21 characters since it's always at least \'http://localhost\'")]
    [Url]
    public string DataServerUrl { get; } = dataServerUrl;

    [Required]
    [MinLength(MinGitBranchNameLength)]
    [MaxLength(MaxGitBranchNameLength)]
    [RegularExpression(GitBranchValidationRegex, ErrorMessage = "Invalid branch name")]
    public string BranchName { get; } = branchName;

    [Required]
    [IsValidProjectName]
    public string ProjectName { get; } = projectName;

    [Required]
    [MinLength(GitConstants.MinBaseCommitGitHubUrlLength, ErrorMessage = GitConstants.BaseCommitGitHubUrlShorterErrorMessage)]
    [MaxLength(GitConstants.MaxCommitGitHubUrlLength)]
    [Url]
    [RegularExpression(GitConstants.BaseGitHubUrlCheckRegex, ErrorMessage = "Invalid base GitHub commit URL")]
    public string BaseCommitGitHubUrl { get; } = baseCommitGitHubUrl;

    [Required]
    [MinLength(1)]
    public List<MetricOperationModel> MetricOperations { get; } = metricOperations;
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