using System.Text.Json;
using API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ExceptionHandling
    {
        public readonly RequestDelegate _next;
        public readonly ILogger<ExceptionHandling> _logger;
        public readonly IHostEnvironment _env;
        public ExceptionHandling(
                RequestDelegate next, 
                ILogger<ExceptionHandling> logger, 
                IHostEnvironment env)
        {
           _next = next;
           _logger = logger;
           _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = new ProblemDetails
            {
                Status = 500,
                Detail = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null,
                Title = ex.Message 
            };
            var options =  new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
            }
        }    
    }
}
