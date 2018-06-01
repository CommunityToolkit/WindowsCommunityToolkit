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

using System.Threading;
using System.Threading.Tasks;
using static Microsoft.Toolkit.Services.MicrosoftGraph.MicrosoftGraphEnums;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Platform abstraction.
    /// </summary>
    public interface IOneDriveStorageItemPlatform
    {
        /// <summary>
        /// Gets the thumbnail's item.
        /// </summary>
        object ThumbNail { get; }

        /// <summary>
        /// Retrieves a thumbnail image for the file
        /// </summary>
        /// <param name="optionSize"> A value from the enumeration that specifies the size of the image to retrieve. Small ,Medium, Large</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>When this method completes, return a stream containing the thumbnail, or null if no thumbnail are available</returns>
        Task<object> GetThumbnailAsync(ThumbnailSize optionSize = ThumbnailSize.Small, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///  Determines whether the current IStorageItem matches the specified StorageItemTypes value.
        /// </summary>
        /// <param name="type">The value to match against.</param>
        /// <returns>True if the IStorageItem matches the specified value; otherwise false.</returns>
        bool IsOfType(object type);
    }
}
