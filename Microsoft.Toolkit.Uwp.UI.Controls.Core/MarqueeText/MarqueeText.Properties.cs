using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Control that displays Text in a Marquee style.
    /// </summary>
    public partial class MarqueeText
    {
        private static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MarqueeText), new PropertyMetadata(null));

        private static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register(nameof(Speed), typeof(double), typeof(MarqueeText), new PropertyMetadata(32d));

        private static readonly DependencyProperty IsRepeatingProperty =
            DependencyProperty.Register(nameof(IsRepeating), typeof(bool), typeof(MarqueeText), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the text being displayed in Marquee.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the speed the text moves in the Marquee.
        /// </summary>
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the marquee scroll repeats.
        /// </summary>
        public bool IsRepeating
        {
            get { return (bool)GetValue(IsRepeatingProperty); }
            set { SetValue(IsRepeatingProperty, value); }
        }
    }
}
