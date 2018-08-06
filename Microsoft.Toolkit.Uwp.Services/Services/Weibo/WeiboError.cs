using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Weibo
{
    public class WeiboError
    {
        [JsonProperty("error_code")]
        public int Code { get; set; }

        [JsonProperty("error")]
        public string Message { get; set; }
    }
}