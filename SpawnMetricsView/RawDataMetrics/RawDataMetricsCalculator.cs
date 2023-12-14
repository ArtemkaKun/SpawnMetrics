using MetricRecordModel;

namespace SpawnMetricsView.RawDataMetrics;

public sealed class RawDataMetricsCalculator
{
    private const string LogTimePropertyName = nameof(MetricRecord.LogTimeUtc);
    private const string CommitGitHubUrlPropertyName = nameof(MetricRecord.CommitGitHubUrl);
    private const string CommitMessagePropertyName = nameof(MetricRecord.CommitMessage);

    private readonly Dictionary<string, Type> _rawDataColumns = CreateDefaultColumns();
    private readonly List<Dictionary<string, object>> _rawDataRows = [];

    public (Dictionary<string, Type> columns, List<Dictionary<string, object>> rows) RecalculateRawData(List<MetricRecord> metricRecords)
    {
        var logTimeToMetricRecordMap = metricRecords.GroupBy(x => x.LogTimeUtc).ToDictionary(x => x.Key, x => x.ToList());

        _rawDataRows.Clear();

        foreach (var (logTime, metrics) in logTimeToMetricRecordMap)
        {
            var firstMetric = metrics[0];

            var dataRow = new Dictionary<string, object>
            {
                { LogTimePropertyName, logTime },
                { CommitGitHubUrlPropertyName, firstMetric.CommitGitHubUrl },
                { CommitMessagePropertyName, firstMetric.CommitMessage }
            };

            foreach (var metric in metrics)
            {
                AddMetricColumn(metric, dataRow);
            }

            _rawDataRows.Add(dataRow);
        }

        return (_rawDataColumns, _rawDataRows);
    }

    private static Dictionary<string, Type> CreateDefaultColumns()
    {
        var metricRecordType = typeof(MetricRecord);

        return new Dictionary<string, Type>
        {
            {
                LogTimePropertyName, metricRecordType.GetProperty(LogTimePropertyName)?.PropertyType ?? typeof(DateTime)
            },
            {
                CommitGitHubUrlPropertyName, metricRecordType.GetProperty(CommitGitHubUrlPropertyName)?.PropertyType ?? typeof(string)
            },
            {
                CommitMessagePropertyName, metricRecordType.GetProperty(CommitMessagePropertyName)?.PropertyType ?? typeof(string)
            }
        };
    }

    private void AddMetricColumn(MetricRecord metric, Dictionary<string, object> dataRow)
    {
        var columnName = $"{metric.Name} ({metric.Units})";

        if (int.TryParse(metric.Value, out var valueInt))
        {
            dataRow.Add(columnName, valueInt);
            _rawDataColumns.TryAdd(columnName, typeof(int));
        }
        else if (float.TryParse(metric.Value, out var valueFloat))
        {
            dataRow.Add(columnName, valueFloat);
            _rawDataColumns.TryAdd(columnName, typeof(float));
        }
        else
        {
            dataRow.Add(columnName, metric.Value);
            _rawDataColumns.TryAdd(columnName, typeof(string));
        }
    }
}