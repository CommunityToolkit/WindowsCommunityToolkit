using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers.CameraHelper;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Camera Control to preview video. Can subscribe to video frames, software bitmap when they arrive.
    /// </summary>
    public partial class CameraPreview
    {
        /// <summary>
        /// Event raised when a new frame arrives.
        /// </summary>
        public event EventHandler<FrameEventArgs> FrameArrived;

        /// <summary>
        /// Event raised when camera preview fails.
        /// </summary>
        public event EventHandler<PreviewFailedEventArgs> PreviewFailed;
    }
}
