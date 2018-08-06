using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Weibo
{
    public class WeiboUser
    {
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
    }
}