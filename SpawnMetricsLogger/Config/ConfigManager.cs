using System.Text.Json;
using MiniValidation;

namespace SpawnMetricsLogger.Config;

public static class ConfigManager
{
    public static ConfigModel? TryReadConfigFromFile(string configFilePath)
    {
        if (File.Exists(configFilePath) == false)
        {
            Console.WriteLine($"Config file {configFilePath} doesn't exist");
            return null;
        }

        var config = JsonSerializer.Deserialize<ConfigModel?>(File.ReadAllText(configFilePath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (config == null)
        {
            Console.WriteLine("Failed to deserialize config");
            return null;
        }

        var isValid = MiniValidator.TryValidate(config, out var errors);

        if (isValid == false)
        {
            Console.WriteLine("Config is invalid:");

            foreach (var error in errors)
            {
                Console.WriteLine(error.Key);

                foreach (var errorMessage in error.Value)
                {
                    Console.WriteLine(errorMessage);
                }
            }

            return null;
        }

        return config;
    }
}