// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Determines how file details panel is displayed in the <see cref="SharePointFileList"/> control.
    /// </summary>
    public enum DetailPaneDisplayMode
    {
        /// <summary>
        /// Hide show DetailPane
        /// </summary>
        Disabled,

        /// <summary>
        /// Show DetailPane aside
        /// </summary>
        Side,

        /// <summary>
        /// Show DetailPane at bottom
        /// </summary>
        Bottom,

        /// <summary>
        /// Show DetailPane over list
        /// </summary>
        Full
    }
}
