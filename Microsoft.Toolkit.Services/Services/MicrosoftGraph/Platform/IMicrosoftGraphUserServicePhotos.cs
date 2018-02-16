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

namespace Microsoft.Toolkit.Services.MicrosoftGraph.Platform
{
    /// <summary>
    /// Platform-specific implementation to retrieve graph photos.
    /// </summary>
    public interface IMicrosoftGraphUserServicePhotos
    {
        /// <summary>
        /// Overload for returning photo stream.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Stream conttaining photo.</returns>
        Task<object> GetPhotoAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Overload for returning photo stream.
        /// </summary>
        /// <returns>Stream conttaining photo.</returns>
        Task<object> GetPhotoAsync();
    }
}
