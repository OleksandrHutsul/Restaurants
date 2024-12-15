using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System;
using Xunit;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddleTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrow_ShouldCallNextDelegete()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once());
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrow_ShouldSetStatusCode404()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        await middleware.InvokeAsync(context, _ => throw notFoundException);

        context.Response.StatusCode.Should().Be(404);
    }

    [Fact()]
    public async Task InvokeAsync_WhenForbidExceptionThrow_ShouldSetStatusCode403()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new ForbidExeption();

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(403);
    }

    [Fact()]
    public async Task InvokeAsync_WhenGeneralExceptionThrow_ShouldSetStatusCode500()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception();

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(500);
    }
}