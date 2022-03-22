using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A Control that displays Text in a Marquee style.
    /// </summary>
    [ContentProperty(Name = nameof(Text))]
    public partial class MarqueeText : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarqueeText"/> class.
        /// </summary>
        public MarqueeText()
        {
            DefaultStyleKey = typeof(MarqueeText);
        }
    }
}
