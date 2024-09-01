using System.Collections;

namespace TacTicA.Common.Exceptions;

/// <summary>
///  This exception should be thrown if the entity cannot be found in the data source.
/// </summary>
[Serializable]
public class EntityNotFoundException : DataAccessException
{
    /// <summary>
    /// Entity Id.
    /// </summary>
    public string Id
    {
        get => GetProperty<string>() ?? string.Empty;
        private set => SetProperty(value);
    }

    /// <summary>
    /// Entity type.
    /// </summary>
    public string EntityType
    {
        get => GetProperty<string>() ?? string.Empty;
        private set => SetProperty(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class with a specified error message.
    /// </summary>
    /// <param name="id">Entity Identifier.</param>
    /// <param name="entityType">Type of object.</param>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string id, string entityType, string message) : base(message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        Id = id;
        EntityType = entityType;
    }
}