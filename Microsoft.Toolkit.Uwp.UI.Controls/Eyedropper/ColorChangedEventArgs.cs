using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides event data for the ColorChanged event.
    /// </summary>
    public sealed class ColorChangedEventArgs
    {
        /// <summary>
        /// Gets the color that is currently selected in the control.
        /// </summary>
        public Color NewColor { get; internal set; }

        /// <summary>
        /// Gets the color that was previously selected in the control.
        /// </summary>
        public Color OldColor { get; internal set; }
    }
}
