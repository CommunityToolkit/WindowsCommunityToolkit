// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// The method that each instance of <see cref="AttachedCardShadow"/> uses when clipping its inner content.
    /// </summary>
    public enum InnerContentClipMode
    {
        /// <summary>
        /// Do not clip inner content.
        /// </summary>
        None,

        /// <summary>
        /// Use <see cref="Windows.UI.Composition.CompositionMaskBrush"/> to clip inner content.
        /// </summary>
        /// <remarks>
        /// This mode has better performance than <see cref="CompositionGeometricClip"/>.
        /// </remarks>
        CompositionMaskBrush,

        /// <summary>
        /// Use <see cref="Windows.UI.Composition.CompositionGeometricClip"/> to clip inner content.
        /// </summary>
        /// <remarks>
        /// Content clipped in this mode will have smoother corners than when using <see cref="CompositionMaskBrush"/>.
        /// </remarks>
        CompositionGeometricClip
    }
}
