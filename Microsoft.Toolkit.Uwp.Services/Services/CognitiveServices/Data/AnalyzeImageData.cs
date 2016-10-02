using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// represents the analyze image request result
    /// </summary>
    public class AnalyzeImageData
    {
        /// <summary>
        /// Gets or sets Categories
        /// </summary>
        [JsonProperty("categories")]
        public Category[] Categories { get; set; }

        /// <summary>
        /// Gets or sets Adult
        /// </summary>
        [JsonProperty("adult")]
        public Adult Adult { get; set; }

        /// <summary>
        /// Gets or sets Tags
        /// </summary>
        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [JsonProperty("description")]
        public Description Description { get; set; }

        /// <summary>
        /// Gets or sets RequestId
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets Metadata
        /// </summary>
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        /// <summary>
        /// Gets or sets Faces
        /// </summary>
        [JsonProperty("faces")]
        public Face[] Faces { get; set; }

        /// <summary>
        /// Gets or sets Color
        /// </summary>
        [JsonProperty("color")]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets ImageType
        /// </summary>
        [JsonProperty("imageType")]
        public Imagetype ImageType { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return VisionServiceJsonHelper.JsonSerialize(this);
        }
    }

    /// <summary>
    /// represents class Adult
    /// </summary>
    public class Adult
    {
        /// <summary>
        /// Gets or sets IsAdultContent
        /// </summary>
        [JsonProperty("isAdultContent")]
        public bool IsAdultContent { get; set; }

        /// <summary>
        /// Gets or sets IsRacyContent
        /// </summary>
        [JsonProperty("isRacyContent")]
        public bool IsRacyContent { get; set; }

        /// <summary>
        /// Gets or sets AdultScore
        /// </summary>
        [JsonProperty("adultScore")]
        public float AdultScore { get; set; }

        /// <summary>
        /// Gets or sets RacyScore
        /// </summary>
        [JsonProperty("racyScore")]
        public float RacyScore { get; set; }
    }

    /// <summary>
    /// represents class Description
    /// </summary>
    public class Description
    {
        /// <summary>
        /// Gets or sets Tags
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets Captions
        /// </summary>
        [JsonProperty("captions")]
        public Caption[] Captions { get; set; }
    }

    /// <summary>
    /// represents class Caption
    /// </summary>
    public class Caption
    {
        /// <summary>
        /// Gets or sets Text
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets Confidence
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }
    }

    /// <summary>
    /// represents class Color
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Gets or sets DominantColorForeground
        /// </summary>
        [JsonProperty("dominantColorForeground")]
        public string DominantColorForeground { get; set; }

        /// <summary>
        /// Gets or sets DominantColorBackground
        /// </summary>
        [JsonProperty("dominantColorBackground")]
        public string DominantColorBackground { get; set; }

        /// <summary>
        /// Gets or sets DominantColors
        /// </summary>
        [JsonProperty("dominantColors")]
        public string[] DominantColors { get; set; }

        /// <summary>
        /// Gets or sets AccentColor
        /// </summary>
        [JsonProperty("accentColor")]
        public string AccentColor { get; set; }

        /// <summary>
        /// Gets or sets IsBwImg
        /// </summary>
        [JsonProperty("isBWImg")]
        public bool IsBwImg { get; set; }
    }

    /// <summary>
    /// represents class Imagetype
    /// </summary>
    public class Imagetype
    {
        /// <summary>
        /// Gets or sets ClipArtType
        /// </summary>
        [JsonProperty("clipArtType")]
        public int ClipArtType { get; set; }

        /// <summary>
        /// Gets or sets LineDrawingType
        /// </summary>
        [JsonProperty("lineDrawingType")]
        public int LineDrawingType { get; set; }
    }

    /// <summary>
    /// represents class Category
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Score
        /// </summary>
        [JsonProperty("score")]
        public float Score { get; set; }

        /// <summary>
        /// Gets or sets Detail
        /// </summary>
        [JsonProperty("detail")]
        public Detail Detail { get; set; }
    }

    /// <summary>
    /// represents class Detail
    /// </summary>
    public class Detail
    {
        /// <summary>
        /// Gets or sets Celebrities
        /// </summary>
        [JsonProperty("celebrities")]
        public Celebrity[] Celebrities { get; set; }
    }

    /// <summary>
    /// represents class Celebrity
    /// </summary>
    public class Celebrity
    {
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets FaceRectangle
        /// </summary>
        [JsonProperty("faceRectangle")]
        public Facerectangle FaceRectangle { get; set; }

        /// <summary>
        /// Gets or sets Confidence
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }
    }

    /// <summary>
    /// represents class Facerectangle
    /// </summary>
    public class Facerectangle
    {
        /// <summary>
        /// Gets or sets Left
        /// </summary>
        [JsonProperty("left")]
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets Top
        /// </summary>
        [JsonProperty("top")]
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets Width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets Height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }
    }

    /// <summary>
    /// represents class Face
    /// </summary>
    public class Face
    {
        /// <summary>
        /// Gets or sets Age
        /// </summary>
        [JsonProperty("age")]
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        [JsonProperty("gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets FaceRectangle
        /// </summary>
        [JsonProperty("faceRectangle")]
        public Facerectangle FaceRectangle { get; set; }
    }
}
