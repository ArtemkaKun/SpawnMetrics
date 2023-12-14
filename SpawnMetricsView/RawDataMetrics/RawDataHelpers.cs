namespace SpawnMetricsView.RawDataMetrics;

public static class RawDataHelpers
{
    public static string GetCommitHashFromGitHubUrl(string url)
    {
        return url.Split('/').Last();
    }

    public static string GetColumnPropertyExpression(string name, Type type)
    {
        var expression = $"""it["{name}"].ToString()""";

        if (type == typeof(int))
        {
            return $"int.Parse({expression})";
        }

        if (type == typeof(float))
        {
            return $"float.Parse({expression})";
        }

        if (type == typeof(DateTime))
        {
            return $"DateTime.Parse({expression})";
        }

        return expression;
    }
}