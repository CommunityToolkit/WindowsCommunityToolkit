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
        /// Returns all available Frame Source Groups on the device.
        /// </summary>
        /// <returns>A a list of MediaFrameSourceGroup objects <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<IReadOnlyList<MediaFrameSourceGroup>> GetAllAvailableFrameSourceGroups()
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
                Debug.WriteLine("Failed to get frame sources." + ex.Message);
            }

            return _frameSourceGroups;
        }

        /// <summary>
        /// Returns first available Frame Source Group.
        /// </summary>
        /// <returns>>A MediaFrameSourceGroup<see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<MediaFrameSourceGroup> GetFirstAvailableFrameSourceGroup()
        {
            if (_frameSourceGroups == null)
            {
               _frameSourceGroups = await GetAllAvailableFrameSourceGroups();
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
