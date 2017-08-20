using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class LandingPageResource
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("links")]
        public LandingPageLink[] Links { get; set; }
    }

}
