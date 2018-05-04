using System;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Arguments relating to a file selected event of SharePointFiles control
    /// </summary>
    public class FileSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets selected file
        /// </summary>
        public DriveItem FileSelected { get; set; }
    }
}
