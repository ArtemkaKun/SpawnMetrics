using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using SpawnMetricsStorage.Models.ProjectName;

namespace SpawnMetricsStorage.Models;

// NOTE: This class can be readonly structure instead of class, which will be more convenient dues to the logic.
// Unfortunately, MiniValidation lib, that used to validate this structure, doesn't support validation of value types.
// NOTE: Used as structure for request, so this struct serialized by JSON serializers.
[UsedImplicitly]
public sealed class GetAllMetricsRequestParameters
{
    [Required]
    [IsValidProjectName]
    public required string ProjectName { get; init; }
}