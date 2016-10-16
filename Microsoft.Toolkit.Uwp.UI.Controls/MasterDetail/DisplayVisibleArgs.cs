namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    ///     The event args for DisplayVisibleChanged event
    /// </summary>
    public class DisplayVisibleArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DisplayVisibleArgs" /> class.
        /// </summary>
        /// <param name="displayVisible">The visible display.</param>
        internal DisplayVisibleArgs(MasterDetailDisplayVisible displayVisible)
        {
            DisplayVisible = displayVisible;
        }

        /// <summary>
        ///     Gets or sets the visible display.
        /// </summary>
        /// <value>
        ///     The visible display.
        /// </value>
        public MasterDetailDisplayVisible DisplayVisible { get; set; }
    }
}