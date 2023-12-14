using MetricRecordModel;

namespace SpawnMetricsView.RawDataMetrics;

public sealed class RawDataMetricsCalculator
{
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
                { RawDataConstants.LogTimePropertyName, logTime },
                { RawDataConstants.CommitGitHubUrlPropertyName, firstMetric.CommitGitHubUrl },
                { RawDataConstants.CommitMessagePropertyName, firstMetric.CommitMessage }
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
                RawDataConstants.LogTimePropertyName, metricRecordType.GetProperty(RawDataConstants.LogTimePropertyName)?.PropertyType ?? typeof(DateTime)
            },
            {
                RawDataConstants.CommitGitHubUrlPropertyName, metricRecordType.GetProperty(RawDataConstants.CommitGitHubUrlPropertyName)?.PropertyType ?? typeof(string)
            },
            {
                RawDataConstants.CommitMessagePropertyName, metricRecordType.GetProperty(RawDataConstants.CommitMessagePropertyName)?.PropertyType ?? typeof(string)
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