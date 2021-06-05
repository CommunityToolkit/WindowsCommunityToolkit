// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media.Surface
{
    /// <summary>
    /// Represents the core interface for interfaces which render onto ICompositionSurface.
    /// </summary>
    public interface IRenderSurface : IDisposable
    {
        /// <summary>
        /// Gets the CompositionGenerator.
        /// </summary>
        ICompositionGenerator Generator { get; }

        /// <summary>
        /// Gets the Surface.
        /// </summary>
        ICompositionSurface Surface { get; }

        /// <summary>
        /// Gets the Surface Size.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Redraws the surface.
        /// </summary>
        void Redraw();

        /// <summary>
        /// Resizes the surface to the specified size.
        /// </summary>
        /// <param name="size">New size of the surface</param>
        void Resize(Size size);
    }
}
