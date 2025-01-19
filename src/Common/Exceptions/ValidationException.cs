using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shifty.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string , List<string>>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures.GroupBy(e => e.PropertyName , e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName     = failureGroup.Key;
                var propertyFailures = failureGroup.ToList();

                Errors.Add(propertyName , propertyFailures);
            }
        }

        public Dictionary<string , List<string>> Errors { get; }
    }
}