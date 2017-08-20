using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{

    public class LandingPageLink
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

}
