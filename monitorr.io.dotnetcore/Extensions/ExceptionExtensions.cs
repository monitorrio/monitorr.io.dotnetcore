using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace monitorr.io.core.Extensions
{
    public static class ExceptionExtensions
    {
        public static async Task MonitorAsync(this Exception exception, Guid logId, HttpContext context)
        {
            var client = new MonitorrClient();
            await client.LogAsync(logId, context, exception, true);
        }

        public static void Monitor(this Exception exception, Guid logId, HttpContext context)
        {
            var client = new MonitorrClient();
            client.Log(logId, context, exception, true);
        }
    }
}
