using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace monitorr.io.core
{
    public interface IMonitorrClient
    {
        string Version { get; set; }
        Task LogAsync(ErrorModel errorModel);
        Task LogAsync(Guid logId, HttpContext context, Exception exception = null, 
            bool isCustom = false, IDictionary<string, string> additionalData = null);
    }
}