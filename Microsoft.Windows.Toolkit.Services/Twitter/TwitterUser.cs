using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    public class TwitterUser
    {
        [JsonProperty("id_str")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
    }
}
