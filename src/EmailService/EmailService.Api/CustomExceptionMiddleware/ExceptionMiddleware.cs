using System;
using System.Net;
using System.Threading.Tasks;
using EmailService.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EmailService.Api.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly CustomExceptionMapper customExceptionMapper;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occured went wrong: {ex}");
                try
                {
                    await HandleExceptionAsync(httpContext, ex);
                }
                catch (Exception customExceptionMapperException)
                {
                    _logger.LogError($"Unexpected error: {customExceptionMapperException}");

                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var errorData = customExceptionMapper.MapErrorToErrorDetails(exception);
            context.Response.StatusCode = errorData.StatusCode;

            return context.Response.WriteAsync(errorData.ToString());
        }
    }
}