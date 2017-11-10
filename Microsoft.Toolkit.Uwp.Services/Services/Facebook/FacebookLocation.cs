using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    public class FacebookLocation
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}