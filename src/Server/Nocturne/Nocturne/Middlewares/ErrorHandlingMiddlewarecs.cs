﻿using FluentValidation;
using Nocturne.Models;
using System.Net;
using System.Text.Json;

namespace Nocturne.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }

        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
        {
            string? result = null;
            switch (exception)
            {
                case RestException re:
                    context.Response.StatusCode = (int)re.Code;
                    result = JsonSerializer.Serialize(new
                    {
                        errors = re.Errors
                    });
                    break;
                case ValidationException e:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    logger.LogError(e, "Not valid data");
                    result = JsonSerializer.Serialize(new
                    {
                        errors = e.Errors
                    });
                    break;
                case Exception e:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    logger.LogError(e, "Unhandled Exception");
                    result = JsonSerializer.Serialize(new
                    {
                        errors = "Server error"
                    });
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }
    }
}
