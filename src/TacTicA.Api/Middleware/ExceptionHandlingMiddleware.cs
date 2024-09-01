using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using FluentValidation.Results;
using System.Text.Json;
using TacTicA.Common.Exceptions;
using TacTicA.Common.Constants;
using TacTicA.Common.Dto.Errors;

namespace TacTicA.ApiGateway.Middleware;

/// <summary>
/// Exception handling middleware.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="next">Request delegate.</param>
    /// <exception cref="ArgumentNullException">Throws if any of params is null.</exception>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Invoke method. For handling exception middleware.
    /// </summary>
    /// <param name="httpContext">Http Context.</param>
    /// <returns>Task.</returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            var (errorResponse, statusCode) = Process(exception);

            httpContext.Response.ContentType = MimeType.ApplicationJson;
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    private (ErrorResponse ErrorResponse, int StatusCode) Process(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => ProcessValidationException(validationException),
            EntityNotFoundException entityNotFoundException => ProcessEntityNotFoundException(entityNotFoundException),
            ActioException actioException => ProcessActioException(actioException),
            // TODO Add more Business exceptions ...
            _ => ProcessException(exception),
        };
    }

    private (ErrorResponse ErrorResponse, int StatusCode) ProcessException(Exception exception)
    {
        _logger.LogError(exception, "Unexpected exception");

        return (new ErrorResponse
        {
            Type = ErrorCode.AsSystem,
            Code = ErrorCode.AsSystem,
            Message = "Unhandled system error",
            Details = new ErrorDetailsDto
            {
                Errors = new ErrorDto[]
                {
                        new()
                        {
                            Name = "Exception",
                            Message = exception.ToString()
                        }
                }
            }
        }, StatusCodes.Status500InternalServerError);
    }

    private (ErrorResponse ErrorResponse, int StatusCode) ProcessActioException(ActioException actioException)
    {
        _logger.LogTrace(actioException, "ActioException exception");

        return (new ErrorResponse
        {
            Type = ErrorCode.AsBussiness,
            Code = actioException.Code ?? ErrorCode.AsBussiness,
            Message = actioException.Message
        }, StatusCodes.Status400BadRequest);
    }

    private (ErrorResponse ErrorResponse, int StatusCode) ProcessEntityNotFoundException(EntityNotFoundException entityNotFoundException)
    {
        _logger.LogTrace(entityNotFoundException, "EntityNotFoundException exception");

        return (new ErrorResponse
        {
            Type = ErrorCode.AsBussiness,
            Code = ErrorCode.AsBussiness, // TODO Code might specify more exact error.
            Message = entityNotFoundException.Message
        }, StatusCodes.Status404NotFound);
    }

    private (ErrorResponse ErrorResponse, int StatusCode) ProcessValidationException(ValidationException validationException)
    {
        _logger.LogDebug(validationException, "Validation exception");

        return (new ErrorResponse
        {
            Type = ErrorCode.AsValidation,
            Code = ErrorCode.AsValidation, // TODO Code might specify more exact error.
            Message = validationException.Message,
            Details = GetDetails(validationException.Errors)
        }, StatusCodes.Status400BadRequest);
    }

    private static ErrorDetailsDto? GetDetails(IEnumerable<ValidationFailure>? validationFailures)
    {
        if (validationFailures is null)
        {
            return null;
        }

        var result = validationFailures
            .Select(x => new ErrorDto
            {
                Name = JsonNamingPolicy.CamelCase.ConvertName(x.PropertyName),
                Message = string.Concat(validationFailures.Select(e => e.ErrorMessage))
            })
            .ToArray();

        return new ErrorDetailsDto
        {
            Errors = result
        };
    }
}