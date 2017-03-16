using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace monitorr.io.core
{
    public sealed class MonitorrClient : IMonitorrClient
    {
        private JsonSerializerSettings _serializationSettings;
        public string Version { get; set; } = "1";

        private HttpClient _httpClient;
        private string _postUrl;
        private Uri _baseUri;

        public MonitorrClient()
        {
            Initialize();
        }

        private void Initialize()
        {
            _httpClient = new HttpClient();

#if RELEASE
            _baseUri = new Uri("https://log.monitorr.io");
#elif STAGING
            _baseUri = new Uri("https://staging-log.monitorr.io");
#else
            _baseUri = new Uri("http://localhost:1900");
#endif
            _httpClient.BaseAddress = _baseUri;
            _postUrl = $"/v{Version}/errors";
            _serializationSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
        }

        public async Task LogAsync(Guid logId, HttpContext context, Exception exception = null, bool isCustom = false)
        {
            var errorModel = ErrorModelCreator.Create(logId, context, exception, isCustom);
            var json = JsonConvert.SerializeObject(errorModel, _serializationSettings);
            await _httpClient.PostAsync(_postUrl, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public async Task LogAsync(ErrorModel errorModel)
        {
            var json = JsonConvert.SerializeObject(errorModel, _serializationSettings);
            await _httpClient.PostAsync(_postUrl, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public void Log(Guid logId, HttpContext context, Exception exception, bool isCustom = false)
        {
            var errorModel = ErrorModelCreator.Create(logId, context, exception, isCustom);
            var json = JsonConvert.SerializeObject(errorModel, _serializationSettings);
            _httpClient.PostAsync(_postUrl, new StringContent(json, Encoding.UTF8, "application/json")).Wait();
        }
    }
}
