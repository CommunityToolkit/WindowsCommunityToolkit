using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Camera Control to preview video. Can subscribe to video frames, software bitmap when they arrive.
    /// </summary>
    public partial class CameraPreview
    {
        /// <summary>
        /// Gets or sets icon for Frame Source Group Button
        /// </summary>
        public ImageSource FrameSourceGroupButtonIcon
        {
            get { return (ImageSource)GetValue(FrameSourceGroupButtonIconProperty); }
            set { SetValue(FrameSourceGroupButtonIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrameSourceGroupButtonIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrameSourceGroupButtonIconProperty =
            DependencyProperty.Register("FrameSourceGroupButtonIcon", typeof(ImageSource), typeof(CameraPreview), new PropertyMetadata(null));
    }
}
