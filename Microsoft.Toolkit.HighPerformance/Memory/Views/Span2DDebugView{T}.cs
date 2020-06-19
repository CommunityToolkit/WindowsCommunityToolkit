// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Microsoft.Toolkit.HighPerformance.Memory.Views
{
    /// <summary>
    /// A debug proxy used to display items for the <see cref="Span2D{T}"/> type.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the input <see cref="Span2D{T}"/> instances.</typeparam>
    internal sealed class Span2DDebugView<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Span2DDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="span">The input <see cref="Span2D{T}"/> instance with the items to display.</param>
        public Span2DDebugView(Span2D<T> span)
        {
            this.Items = span.ToArray();
        }

        /// <summary>
        /// Gets the items to display for the current instance
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public T[,]? Items { get; }
    }
}
