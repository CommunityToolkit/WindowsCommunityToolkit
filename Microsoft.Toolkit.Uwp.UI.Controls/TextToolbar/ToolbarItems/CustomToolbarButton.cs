namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    using System;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An addition custom button to add to the Toolbar, to apply custom formatting.
    /// </summary>
    public class CustomToolbarButton : CustomToolbarItem
    {
        /// <summary>
        /// Gets or sets icon to Attach to Custom Button
        /// </summary>
        public IconElement Icon { get; set; }

        /// <summary>
        /// Gets or sets label of Button, show in Tooltip
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets action for Button to Perform (Must be aware of FormattingType)
        /// </summary>
        public Action Action { get; set; }
    }
}