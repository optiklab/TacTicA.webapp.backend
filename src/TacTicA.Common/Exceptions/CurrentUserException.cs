using System.Collections;

namespace TacTicA.Common.Exceptions;

/// <summary>
/// This exception should be thrown if any problems occurred while accessing the data of current user.
/// </summary>
[Serializable]
public class CurrentUserException : ServiceLayerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public CurrentUserException(string message) : base(message)
    {
    }
}