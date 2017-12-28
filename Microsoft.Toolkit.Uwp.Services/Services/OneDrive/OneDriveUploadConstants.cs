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

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    ///  Class ofr the OneDrive Constants
    /// </summary>
    public class OneDriveUploadConstants
    {
        /// <summary>
        /// Maximum file size for a simple upload
        /// </summary>
        public const int SimpleUploadMaxSize = 4 * 1024 * 1024;

        /// <summary>
        /// Default chunk when uploading a karge file
        /// </summary>
        public const int DefaultMaxChunkSizeForUploadSession = 5 * 1024 * 1024;

        /// <summary>
        /// Chunk size increment
        /// </summary>
        public const int RequiredChunkSizeIncrementForUploadSession = 320 * 1024;
    }
}
