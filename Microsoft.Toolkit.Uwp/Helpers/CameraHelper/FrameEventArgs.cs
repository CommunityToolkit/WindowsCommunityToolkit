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
using System.Threading;
using Windows.Foundation.Metadata;
using Windows.Graphics.Imaging;
using Windows.Media;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// EventArgs to be used by <see cref="CameraHelper"/> FrameArrived Event
    /// </summary>
    public class FrameEventArgs : EventArgs
    {
        private VideoFrame _videoFrame;
        private VideoFrame _videoFrameCopy;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        /// <summary>
        /// Gets Video Frame
        /// </summary>
        public VideoFrame VideoFrame
        {
            get
            {
                _semaphore.Wait();

                // videoFrame could be disposed at any time so we need to create a copy we can use
                // this api is only available on 17134 - so we return the original VideoFrame on older versions
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
