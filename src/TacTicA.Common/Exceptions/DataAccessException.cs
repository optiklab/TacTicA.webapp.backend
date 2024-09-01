using System.Collections;

namespace TacTicA.Common.Exceptions;

/// <summary>
/// This exception should be thrown if any problems occurred while accessing the data, such as accessing a database.
/// </summary>
[Serializable]
public class DataAccessException : ServiceLayerException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DataAccessException(string message) : base(message)
    {
    }
}