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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.Capture.Frames;

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// Helper class to get Frame Source Groups that can be used by Media Capture
    /// </summary>
    public static class FrameSourceGroupsHelper
    {
        private static IReadOnlyList<MediaFrameSourceGroup> _frameSourceGroups;

        /// <summary>
        /// Returns all available Frame Source Groups.
        /// </summary>
        /// <returns>A a list of MediaFrameSourceGroup objects <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<IReadOnlyList<MediaFrameSourceGroup>> GetAllAvailableFrameSourceGroupsAsync()
        {
            try
            {
                // Make sure you have the capability webcam enabled, otherwise it will return null
                if (_frameSourceGroups == null)
                {
                    _frameSourceGroups = await MediaFrameSourceGroup.FindAllAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to get frame sources. Error: {ex.Message}");
            }

            return _frameSourceGroups;
        }

        /// <summary>
        /// Returns first available Frame Source Group.
        /// </summary>
        /// <returns>>A MediaFrameSourceGroup<see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<MediaFrameSourceGroup> GetFirstAvailableFrameSourceGroupAsync()
        {
            if (_frameSourceGroups == null)
            {
               _frameSourceGroups = await GetAllAvailableFrameSourceGroupsAsync();
            }

            var defaultGroup = _frameSourceGroups.FirstOrDefault(g => g.SourceInfos.Any(s => s.SourceKind == MediaFrameSourceKind.Color &&
                                                                             (s.MediaStreamType == MediaStreamType.VideoPreview
                                                                             || s.MediaStreamType == MediaStreamType.VideoRecord)));
            if (defaultGroup == null)
            {
                Debug.WriteLine("No camera available");
            }

            return defaultGroup;
        }
    }
}
