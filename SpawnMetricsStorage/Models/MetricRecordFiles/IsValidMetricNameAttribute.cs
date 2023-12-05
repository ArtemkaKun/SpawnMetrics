using SpawnMetricsStorage.Utils;

namespace SpawnMetricsStorage.Models.MetricRecordFiles;

public sealed class IsValidMetricNameAttribute() : NameValidationAttributeBase(MetricRecordConstants.MinMetricNameLength, MetricRecordConstants.MaxMetricNameLength);