using FluentValidation.Results;

namespace Shifty.Common.Exceptions;

public class ValidationException() : Exception("One or more validation failures have occurred.")
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName     = failureGroup.Key;
            var propertyFailures = failureGroup.ToList();

            Errors.Add(propertyName, propertyFailures);
        }
    }

    public Dictionary<string, List<string>> Errors { get; } = new();
}