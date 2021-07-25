// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Geometry;

namespace Microsoft.Toolkit.Uwp.UI.Media.Geometry
{
    internal interface ICanvasPathGeometry
    {
        /// <summary>
        /// Gets the associated <see cref="CanvasGeometry"/>.
        /// </summary>
        CanvasGeometry Geometry { get; }
    }
}
