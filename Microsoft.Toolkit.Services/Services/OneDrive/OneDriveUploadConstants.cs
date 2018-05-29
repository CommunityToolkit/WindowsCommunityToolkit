// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  Class of the OneDrive Constants
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
