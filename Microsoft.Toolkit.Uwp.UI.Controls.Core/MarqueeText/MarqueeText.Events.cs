using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Control that displays Text in a Marquee style.
    /// </summary>
    public partial class MarqueeText
    {
        /// <summary>
        /// Event raised when the Marquee begins scrolling.
        /// </summary>
        public event EventHandler MarqueeBegan;

        /// <summary>
        /// Event raised when the Marquee stops scrolling for any reason.
        /// </summary>
        public event EventHandler MarqueeStopped;

        /// <summary>
        /// Event raised when the Marquee completes scrolling.
        /// </summary>
        public event EventHandler MarqueeCompleted;

        private void MarqueeText_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= MarqueeText_Unloaded;
            _rootGrid.SizeChanged -= RootGrid_SizeChanged;
        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_rootGrid != null)
            {
                RectangleGeometry clip = new RectangleGeometry();
                clip.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
                _rootGrid.Clip = clip;
            }

            StartMarquee();
        }

        private void StoryBoard_Completed(object sender, object e)
        {
            StopMarque(true);
            MarqueeCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
