using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    public class LandingPageLinks
    {
        [JsonProperty("new-section-title")]
        public string NewSectionTitle { get; set; }

        [JsonProperty("new-samples")]
        public string[] NewSamples { get; set; }

        [JsonProperty("resources")]
        public LandingPageResource[] Resources { get; set; }
    }
}
