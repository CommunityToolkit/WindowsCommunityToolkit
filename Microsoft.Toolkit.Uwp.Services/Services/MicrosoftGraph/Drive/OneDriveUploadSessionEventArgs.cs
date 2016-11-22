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

using System;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// Class OneDriveUploadSessionEventArgs
    /// </summary>
    public class OneDriveUploadSessionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the file's total size
        /// </summary>
        public long UploadSessionTotalSize { get; set; }

        /// <summary>
        /// Gets or sets th size of the upload chunk
        /// </summary>
        public long UploadSessionChunk { get; set; }

        /// <summary>
        /// Gets or sets the remaining size after each upload chunk
        /// </summary>
        public long UploadSessionRemaining { get; set; }

        /// <summary>
        /// Gets or sets the uploaded file
        /// </summary>
        public OneDriveStorageFile File { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveUploadSessionEventArgs"/> class.
        /// </summary>
        /// <param name="uploadSessionTotalSize">The length of the file</param>
        /// <param name="uploadSessionChunk">The chunk size</param>
        /// <param name="uploadSessionRemaining">Remaining size after each upload chunk</param>
        /// <param name="file">Represente the uploaded file</param>
        public OneDriveUploadSessionEventArgs(long uploadSessionTotalSize, long uploadSessionChunk, long uploadSessionRemaining, OneDriveStorageFile file)
        {
            UploadSessionChunk = uploadSessionChunk;
            UploadSessionTotalSize = uploadSessionTotalSize;
            UploadSessionRemaining = uploadSessionRemaining;
            File = file;
        }
    }
}
