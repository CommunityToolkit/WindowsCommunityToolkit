namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows of a Grid control.
    /// </summary>
    public partial class GridSplitter
    {
        /// <summary>
        /// Gets or sets the type of the GridSplitter Columns(Default Value) or Rows
        /// </summary>
        public ResizeDirection ResizeDirection { get; set; }
    }

    /// <summary>
    /// Enumeration used to select the type of Grid Spliter Columns or Rows
    /// </summary>
    public enum ResizeDirection
    {
        /// <summary>
        /// Grid Splitter Column Type (Default Value)
        /// </summary>
        Columns,

        /// <summary>
        /// Grid Splitter Row Type
        /// </summary>
        Rows
    }
}
