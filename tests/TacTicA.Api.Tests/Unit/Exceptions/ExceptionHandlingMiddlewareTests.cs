using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TacTicA.Common.Exceptions;
using Xunit;
using System.Text.Json;
using TacTicA.Common.Dto.Errors;
using AutoFixture;
using TacTicA.ApiGateway.Middleware;
using Microsoft.AspNetCore.Components;

namespace TacTicA.Api.Tests.Unit.Exceptions;

public class ExceptionHandlingMiddlewareTests
{
    private readonly IFixture _fixture;
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly ExceptionHandlingMiddleware _middleware;

    public ExceptionHandlingMiddlewareTests()
    {
        _fixture = new Fixture();
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        _nextMock = new Mock<RequestDelegate>();
        _middleware = new ExceptionHandlingMiddleware(_loggerMock.Object, _nextMock.Object);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExceptionHandlingMiddleware(null!, _nextMock.Object));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenNextIsNull()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExceptionHandlingMiddleware(_loggerMock.Object, null!));
    }

    [Fact]
    public async Task InvokeAsync_CallsNextDelegate_WhenNoExceptionThrown()
    {
        // Arrange
        var contextMock = new DefaultHttpContext();

        // Act
        await _middleware.InvokeAsync(contextMock);

        // Assert
        _nextMock.Verify(next => next(contextMock), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_HandlesValidationException()
    {
        // Arrange
        var validationException = _fixture.Create<ValidationException>();
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(validationException);

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(context.Response.Body);
        Assert.NotNull(errorResponse);
        Assert.NotNull(errorResponse.Details);
        Assert.Equal(3, errorResponse.Details.Errors.Length);
        var errors = validationException.Errors!.ToArray();
        Assert.Equal(FirstCharToLowerCase(errors[0].PropertyName), errorResponse.Details.Errors[0].Name);
        Assert.Equal(FirstCharToLowerCase(errors[1].PropertyName), errorResponse.Details.Errors[1].Name);
        Assert.Equal(FirstCharToLowerCase(errors[2].PropertyName), errorResponse.Details.Errors[2].Name);
    }

    [Fact]
    public async Task InvokeAsync_HandlesEntityNotFoundException()
    {
        // Arrange
        var entityNotFoundException = _fixture.Create<EntityNotFoundException>();
        var contextMock = new DefaultHttpContext();
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(entityNotFoundException);

        // Act
        await _middleware.InvokeAsync(contextMock);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, contextMock.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesUnexpectedException()
    {
        // Arrange
        var unexpectedException = _fixture.Create<Exception>();
        var contextMock = new DefaultHttpContext();
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(unexpectedException);

        // Act
        await _middleware.InvokeAsync(contextMock);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, contextMock.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_HandlesActioException()
    {
        // Arrange
        var actioException = _fixture.Create<ActioException>();
        actioException.Code = "MyCode";
        var contextMock = new DefaultHttpContext();
        contextMock.Response.Body = new MemoryStream();
        _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(actioException);

        // Act
        await _middleware.InvokeAsync(contextMock);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, contextMock.Response.StatusCode);

        contextMock.Response.Body.Seek(0, SeekOrigin.Begin);
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(contextMock.Response.Body);
        Assert.NotNull(errorResponse);
        Assert.NotNull(errorResponse.Message);
        Assert.Equal("MyCode", errorResponse.Code);
    }

    private static string FirstCharToLowerCase(string str)
    {
        if (!string.IsNullOrEmpty(str) && char.IsUpper(str[0]))
        {
            return str.Length == 1 ? char.ToLower(str[0]).ToString() : char.ToLower(str[0]) + str[1..];
        }

        return str;
    }
}