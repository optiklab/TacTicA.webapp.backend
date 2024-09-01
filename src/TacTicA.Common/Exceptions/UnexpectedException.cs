using System.Collections;

namespace TacTicA.Common.Exceptions;

public class UnexpectedException : ServiceLayerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or <b>null</b> if no inner exception is specified.</param>
    public UnexpectedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UnexpectedException(string message) : base(message)
    {
    }
}