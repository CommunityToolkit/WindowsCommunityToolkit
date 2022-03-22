using System;
using Windows.UI.Xaml;

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

        private void MarqueeText_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            StartMarquee();
        }

        private void StoryBoard_Completed(object sender, object e)
        {
            StopMarque(true);
            MarqueeCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
