namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the mode characters are entered in a text box.
    /// </summary>
    public enum InsertKeyMode
    {
        /// <summary>
        /// Honors the Insert key mode.
        /// </summary>
        Default,

        /// <summary>
        /// Forces insertion mode to be 'on' regardless of the Insert key mode.
        /// </summary>
        Insert,

        /// <summary>
        /// Forces insertion mode to be 'off' regardless of the Insert key mode.
        /// </summary>
        Overwrite
    }
}