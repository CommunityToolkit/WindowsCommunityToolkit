// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Helpers
{
    /// <summary>
    /// Represents the types of items available in a directory.
    /// </summary>
    public enum DirectoryItemType
    {
        /// <summary>
        /// The item is neither a file or a folder.
        /// </summary>
        None,

        /// <summary>
        /// Represents a file type item.
        /// </summary>
        File,

        /// <summary>
        /// Represents a folder type item.
        /// </summary>
        Folder
    }
}
