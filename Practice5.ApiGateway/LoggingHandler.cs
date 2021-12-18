using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Practice5.ApiGateway
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;

        public LoggingHandler(IHttpContextAccessor accessor) => _accessor = accessor;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken ct)
        {
            var requestId = _accessor.HttpContext?.Items["requestId"] as string;
            request.Headers.Add("REQ-ID", requestId);

            return base.SendAsync(request, ct);
        }
    }
}