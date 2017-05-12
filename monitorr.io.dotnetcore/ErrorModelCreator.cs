using System;
using System.Collections;
using System.Collections.Generic;
using monitorr.io.core.Detection;
using Microsoft.AspNetCore.Http;

namespace monitorr.io.core
{
    public static class ErrorModelCreator
    {
        public static ErrorModel Create(Guid logId, HttpContext context, Exception exception = null, 
            bool isCustom = false, IDictionary<string, string> additionalData = null)
        {
            return new ErrorModel
            {
                Guid = Guid.NewGuid().ToString(),
                Message = Message(exception),
                Time = DateTime.UtcNow,
                Detail = Detail(exception),
                Source = exception?.Source,
                Type = exception?.GetType().Name,
                Cookies = Cookies(context),
                Form = Form(context),
                Host = context.Request?.Host.Value,
                ServerVariables = ServerVariables(context),
                StatusCode = context.Response?.StatusCode,
                QueryString = QueryString(context),
                Method = context.Request?.Method,
                LogId = logId.ToString(),
                Browser = Browser(context),
                User = User(context),
                Url = context.Request?.Path.Value,
                Severity = GetSeverity(context.Response?.StatusCode),
                IsCustom = isCustom,
                CustomData = additionalData
            };
        }

        private static Severity GetSeverity(int? responseStatusCode)
        {
            if (responseStatusCode.HasValue && responseStatusCode == 500)
            {
                return Severity.Crytical;
            }

            return Severity.Warning;
        }


        private static string User(HttpContext context)
        {
            return context.User?.Identity?.Name;
        }

        private static string Browser(HttpContext context)
        {
            var ua = context?.Request.Headers["User-Agent"];
            if (ua.HasValue)
            {
                return BrowserDetection.Detect(ua);
            }

            return null;
        }

        private static string Message(Exception exception)
        {
            if (exception == null)
            {
                return "Status code is unsuccessful";
            }
            return exception.Message;
        }

        private static string Detail(Exception exception)
        {
            return exception?.ToString();
        }

        private static Dictionary<string, string> Form(HttpContext context)
        {
            try
            {
                return MonitorrHelpers.ToDictionary(context.Request?.Form);
            }
            catch (InvalidOperationException)
            {
            }

            return null;
        }

        private static Dictionary<string, string> QueryString(HttpContext context)
        {
            return MonitorrHelpers.ToDictionary(context.Request?.Query);
        }

        private static Dictionary<string, string> ServerVariables(HttpContext context)
        {
            return MonitorrHelpers.ToDictionary(context.Request?.Headers);
        }

        private static Dictionary<string, string> Cookies(HttpContext context)
        {
            return MonitorrHelpers.ToDictionary(context.Request?.Cookies);
        }
    }
}
