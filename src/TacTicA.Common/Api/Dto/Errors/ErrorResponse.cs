namespace TacTicA.Common.Dto.Errors;

public record ErrorResponse
{
    /// <summary>
    ///  Type of the error. Type can be considered to be "group" or errors.
    ///  It defines the format of the details field and specifies how applications must behave when showing errors to the end user.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Code of the error that allows applications to distinguish between different errors and perform specific actions if needed.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Text description of the error.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Object that holds additional information for the error.
    /// </summary>
    public ErrorDetailsDto? Details { get; set; }
}