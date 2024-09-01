namespace TacTicA.Common.Dto.Errors;

/// <summary>
/// Error, name with message.
/// </summary>
public record ErrorDto
{
    /// <summary>
    /// Name of the field with an error.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Error message.
    /// </summary>
    public required string Message { get; init; }
}