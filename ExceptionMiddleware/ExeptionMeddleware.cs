using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExceptionMiddleware
{
    public class ExeptionMeddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExeptionMeddleware> logger;

        public ExeptionMeddleware(RequestDelegate next, ILogger<ExeptionMeddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
                await context.Response.WriteAsync(ex.StackTrace);
                logger.LogCritical(ex.Message + ex.StackTrace);
            }
        }
    }

    public static class ExeptionMeddlewareExtension
    {
        public static IApplicationBuilder UseExeptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExeptionMeddleware>();
        }
    }
}