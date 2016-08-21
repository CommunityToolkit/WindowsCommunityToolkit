using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// ColumnResizer is a UI control that add the resizing functionality to a Grid Column.
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
