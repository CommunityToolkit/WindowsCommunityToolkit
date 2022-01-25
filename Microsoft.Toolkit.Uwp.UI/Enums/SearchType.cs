// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Indicates a type of search for elements in a visual or logical tree.
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// Depth-first search, where each branch is recursively explored until the end before moving to the next one.
        /// </summary>
        DepthFirst,

        /// <summary>
        /// Breadth-first search, where each depthwise level is completely explored before moving to the next one.
        /// This is particularly useful if the target element to find is known to not be too distant from the starting
        /// point and the whole visual/logical tree from the root is large enough, as it can reduce the traversal time.
        /// </summary>
        BreadthFirst
    }
}
