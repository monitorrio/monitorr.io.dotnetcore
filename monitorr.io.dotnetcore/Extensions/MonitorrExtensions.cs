using System;
using Microsoft.AspNetCore.Builder;

namespace monitorr.io.core.Extensions
{
    public static class MonitorrExtensions
    {
        public static IApplicationBuilder Monitor(this IApplicationBuilder app, Guid logId)
        {
            return app.UseMiddleware<MonitorrMiddleware>(logId);
        }
    }
}
