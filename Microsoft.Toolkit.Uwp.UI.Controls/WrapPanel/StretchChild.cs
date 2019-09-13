namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Options for how to calculate the layout of <see cref="Windows.UI.Xaml.Controls.WrapGrid"/> items.
    /// </summary>
    public enum StretchChild
    {
        /// <summary>
        /// Don't apply any additional stretching logic
        /// </summary>
        None,

        /// <summary>
        /// Make the first child stretch to push any other items to the end
        /// </summary>
        First,

        /// <summary>
        /// Make the last child stretch to fill the available space
        /// </summary>
        Last
    }
}
