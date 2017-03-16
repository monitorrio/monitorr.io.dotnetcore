using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace monitorr.io.core
{
    public static class MonitorrHelpers
    {
        public static Dictionary<string, string> ToDictionary(IHeaderDictionary requestHeaders)
        {
            return requestHeaders?.ToDictionary(k => k.Key, k => requestHeaders[k.Key].ToString());
        }

        public static Dictionary<string, string> ToDictionary(IRequestCookieCollection requestCookies)
        {
            return requestCookies?.ToDictionary(k => k.Key, k => requestCookies[k.Key].ToString());
        }

        public static Dictionary<string, string> ToDictionary(IFormCollection requestForm)
        {
            return requestForm?.ToDictionary(k => k.Key, k => requestForm[k.Key].ToString());
        }

        public static Dictionary<string, string> ToDictionary(IQueryCollection requestQueryString)
        {
            return requestQueryString?.ToDictionary(k => k.Key, k => requestQueryString[k.Key].ToString());
        }
    }
}
