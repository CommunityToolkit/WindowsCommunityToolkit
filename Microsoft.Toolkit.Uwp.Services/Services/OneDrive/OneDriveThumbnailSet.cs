using Microsoft.OneDrive.Sdk;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Class used to store urls to a specific set of thumbnails
    /// </summary>
    public class OneDriveThumbnailSet
    {
        /// <summary>
        /// Gets the url to the small version of the thumbnail
        /// </summary>
        public string Small { get; }

        /// <summary>
        /// Gets the url to the medium version of the thumbnail
        /// </summary>
        public string Medium { get; }

        /// <summary>
        /// Gets the url to the large version of the thumbnail
        /// </summary>
        public string Large { get; }

        /// <summary>
        /// Gets the url to the original version of the thumbnail
        /// </summary>
        public string Source { get; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="OneDriveThumbnailSet"/> class.
        /// </summary>
        /// <param name="set">Original set from OneDrive SDK</param>
        internal OneDriveThumbnailSet(ThumbnailSet set)
        {
            Small = set.Small.Url;
            Medium = set.Medium.Url;
            Large = set.Large.Url;
            Source = set.Source?.Url;
        }
    }
}
