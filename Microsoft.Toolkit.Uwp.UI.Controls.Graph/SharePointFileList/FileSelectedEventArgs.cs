// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Arguments relating to a file selected event of SharePointFiles control
    /// </summary>
    public class FileSelectedEventArgs
    {
        /// <summary>
        /// Gets selected file
        /// </summary>
        public DriveItem FileSelected { get; private set; }

        internal FileSelectedEventArgs(DriveItem fileSelected)
        {
            FileSelected = fileSelected;
        }
    }
}
