using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shared
{
    public class LogContextMiddleware : IMiddleware
    {
        private readonly ILogger<LogContextMiddleware> _logger;

        public LogContextMiddleware(ILogger<LogContextMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestId = ReadRequestId(context);

            using (_logger.BeginScope("{requestId}", requestId))
            {
                context.Items["requestId"] = requestId;
                _logger.LogInformation(requestId);
                await next(context);
            }

            static string ReadRequestId(HttpContext httpContext)
            {
                var requestIdHeader = httpContext.Request.Headers["REQ-ID"].ToString();

                return !string.IsNullOrEmpty(requestIdHeader)
                    ? requestIdHeader
                    : Guid.NewGuid().ToString("N");
            }
        }
    }
}