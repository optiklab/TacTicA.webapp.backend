using FluentValidation.Results;
using System.Collections;

namespace TacTicA.Common.Exceptions;

/// <summary>
/// This exception should be thrown if any validation problems occurred.
/// </summary>
[Serializable]
public class ValidationException : ServiceLayerException
{
    /// <summary>
    /// A list of validation errors.
    /// </summary>
    public IEnumerable<ValidationFailure>? Errors
    {
        get => GetProperty<IEnumerable<ValidationFailure>>();
        private set => SetProperty(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="errors">The list of validation errors.</param>
    public ValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message)
    {
        Errors = errors;
    }
}