using System.Net;
using System.Net.Mime;
using EmployeeManagement.Domain.Dtos;
using FluentValidation;

namespace EmployeeManagement.API.Middleware;

public sealed class ExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<ExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
                throw;

            logger.LogError(
                exception,
                "Unhandled exception processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await WriteResponseAsync(context, exception, environment);
        }
    }

    private static Task WriteResponseAsync(
        HttpContext context,
        Exception exception,
        IWebHostEnvironment environment)
    {
        EmptyResultDto response;

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = EmptyResult.InvalidRequest(string.Join(
                    Environment.NewLine,
                    validationException.Errors
                        .Select(error => error.ErrorMessage)
                        .Distinct()));
                break;

            case BadHttpRequestException badHttpRequestException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = EmptyResult.InvalidRequest(
                    badHttpRequestException.Message);
                break;

            default:
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;
                response = EmptyResult.UnknownError(
                    "Ocurrió un error inesperado.");

                if (environment.IsDevelopment())
                    response.AppendDetails(exception.Message);
                break;
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsJsonAsync(
            response,
            context.RequestAborted);
    }
}
