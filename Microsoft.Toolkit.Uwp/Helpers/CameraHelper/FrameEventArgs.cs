// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Windows.Foundation.Metadata;
using Windows.Graphics.Imaging;
using Windows.Media;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Provides data for the <see cref="CameraHelper.FrameArrived"/> event.
    /// </summary>
    public class FrameEventArgs : EventArgs
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private VideoFrame _videoFrame;
        private VideoFrame _videoFrameCopy;

        /// <summary>
        /// Gets the video frame.
        /// </summary>
        public VideoFrame VideoFrame
        {
            get
            {
                _semaphore.Wait();

                // The VideoFrame could be disposed at any time so we need to create a copy we can use.
                // This API is only available on 17134 so we return the original VideoFrame on older versions.
                if (_videoFrameCopy == null &&
                    ApiInformation.IsMethodPresent("Windows.Media.VideoFrame", "CreateWithSoftwareBitmap", 1) &&
                    _videoFrame != null &&
                    _videoFrame.SoftwareBitmap != null)
                {
                    try
                    {
                        _videoFrameCopy = VideoFrame.CreateWithSoftwareBitmap(SoftwareBitmap.Copy(_videoFrame.SoftwareBitmap));
                    }
                    catch (Exception)
                    {
                    }
                }

                _semaphore.Release();
                return _videoFrameCopy ?? _videoFrame;
            }

            internal set
            {
                _videoFrame = value;
            }
        }
    }
}
