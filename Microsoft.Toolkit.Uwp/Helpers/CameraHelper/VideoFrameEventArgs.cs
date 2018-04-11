using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;

namespace Microsoft.Toolkit.Uwp.Helpers.CameraHelper
{
    /// <summary>
    /// EventArgs to be used by <see cref="CameraHelper"/> VideoFrameArrived Event
    /// </summary>
    public class VideoFrameEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Video Frame</summary>
        public VideoFrame VideoFrame { get; set; }

        /// <summary>
        /// Gets or sets SoftwareBitmap
        /// </summary>
        public SoftwareBitmap SoftwareBitmap { get; set; }
    }
}
