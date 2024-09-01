namespace TacTicA.Common.Dto.Errors;

/// <summary>
/// Additional details of the error.
/// </summary>
public record ErrorDetailsDto
{
    /// <summary>
    /// List of occured errors.m 
    /// </summary>
    public ErrorDto[] Errors { get; init; } = Array.Empty<ErrorDto>();
}