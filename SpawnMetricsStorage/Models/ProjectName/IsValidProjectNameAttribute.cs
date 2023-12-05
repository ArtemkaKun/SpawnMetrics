using SpawnMetricsStorage.Utils;

namespace SpawnMetricsStorage.Models.ProjectName;

public sealed class IsValidProjectNameAttribute() : NameValidationAttributeBase(ProjectNameConstants.MinProjectNameLength, ProjectNameConstants.MaxProjectNameLength);