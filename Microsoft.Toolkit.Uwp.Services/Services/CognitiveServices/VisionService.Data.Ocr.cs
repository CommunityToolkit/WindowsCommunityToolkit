using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{

    public class OcrData
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("textAngle")]
        public float TextAngle { get; set; }

        [JsonProperty("orientation")]
        public string Orientation { get; set; }

        [JsonProperty("regions")]
        public Region[] Regions { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return VisionServiceJsonHelper.Stringify(this);
        }
    }

    public class Region
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        [JsonProperty("lines")]
        public Line[] Lines { get; set; }
    }

    public class Line
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        [JsonProperty("words")]
        public Word[] Words { get; set; }
    }

    public class Word
    {
        [JsonProperty("boundingBox")]
        public string BoundingBox { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
