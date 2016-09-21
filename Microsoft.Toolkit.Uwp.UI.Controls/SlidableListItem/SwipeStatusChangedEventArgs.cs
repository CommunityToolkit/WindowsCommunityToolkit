namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event args for a SwipeStatus changing event
    /// </summary>
    public class SwipeStatusChangedEventArgs
    {
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public SwipeStatus OldValue { get; internal set; }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public SwipeStatus NewValue { get; internal set; }
    }
}
