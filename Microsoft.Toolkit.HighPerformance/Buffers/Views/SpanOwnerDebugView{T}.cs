// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Microsoft.Toolkit.HighPerformance.Buffers.Views
{
    /// <summary>
    /// A debug proxy used to display items for the <see cref="SpanOwner{T}"/> type.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the input <see cref="SpanOwner{T}"/> instances.</typeparam>
    internal sealed class SpanOwnerDebugView<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpanOwnerDebugView{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="spanOwner">The input <see cref="SpanOwner{T}"/> instance with the items to display.</param>
        public SpanOwnerDebugView(SpanOwner<T> spanOwner)
        {
            this.Items = spanOwner.Span.ToArray();
        }

        /// <summary>
        /// Gets the items to display for the current instance
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[]? Items { get; }
    }
}
