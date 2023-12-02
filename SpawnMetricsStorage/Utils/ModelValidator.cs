using MiniValidation;

namespace SpawnMetricsStorage.Utils;

public static class ModelValidator
{
    public static IResult? Validate<T>(T model) where T : class
    {
        var isValid = MiniValidator.TryValidate(model, out var errors);

        if (isValid == false)
        {
            return Results.ValidationProblem(errors);
        }

        return null;
    }
}