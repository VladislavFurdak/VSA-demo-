using FluentValidation.Results;

namespace OutpatientClinic.Api.Shared.Http;

public static class FluentValidationExtensions
{
    public static IReadOnlyDictionary<string, string[]> ToDictionary(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(x => x.ErrorMessage).Distinct().ToArray());
    }
}
