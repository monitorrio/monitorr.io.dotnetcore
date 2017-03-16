using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace monitorr.io.core
{
    public class MonitorrMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Guid _logId;

        private readonly IMonitorrClient _apiClient;

        public MonitorrMiddleware(RequestDelegate next, Guid logId)
        {
            _next = next;
            _logId = logId;
            _apiClient = new MonitorrClient();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode >= 400)
                {
                    await _apiClient.LogAsync(_logId, context);
                }
            }
            catch (Exception exception)
            {
                await _apiClient.LogAsync(_logId, context, exception);
                throw;
            }
        }
    }
}
