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

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Background service interface.
    /// </summary>
    public interface IOneDriveServicePlatform
    {
        /// <summary>
        /// Creates a background download for the current item.
        /// </summary>
        /// <param name="oneDriveId">OneDrive Id.</param>
        /// <param name="destinationFile">Destination storage file.</param>
        /// <param name="completionGroup">Completion group.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Storage file.</returns>
        Task<object> CreateBackgroundDownloadForItemAsync(string oneDriveId, object destinationFile, object completionGroup = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
