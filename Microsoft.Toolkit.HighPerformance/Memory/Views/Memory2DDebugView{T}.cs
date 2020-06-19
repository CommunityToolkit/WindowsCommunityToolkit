// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Microsoft.Toolkit.HighPerformance.Memory.Views
{
    /// <summary>
    /// A debug proxy used to display items for the <see cref="Memory2D{T}"/> type.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the input <see cref="Memory2D{T}"/> instances.</typeparam>
    internal sealed class Memory2DDebugView<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Memory2DDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="memory">The input <see cref="Memory2D{T}"/> instance with the items to display.</param>
        public Memory2DDebugView(Memory2D<T> memory)
        {
            this.Items = memory.ToArray();
        }

        /// <summary>
        /// Gets the items to display for the current instance
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public T[,]? Items { get; }
    }
}
