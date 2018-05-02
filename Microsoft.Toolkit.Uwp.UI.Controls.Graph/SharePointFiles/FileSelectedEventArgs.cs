using Microsoft.Graph;
using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public class FileSelectedEventArgs : EventArgs
    {
        public DriveItem FileSelected { get; set; }
    }
}
