// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
