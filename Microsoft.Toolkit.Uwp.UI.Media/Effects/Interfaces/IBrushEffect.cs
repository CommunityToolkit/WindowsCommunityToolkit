// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Composition;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// The base <see langword="interface"/> for all the builder effects to be used in a <see cref="CompositionBrush"/>.
    /// </summary>
    public interface IBrushEffect
    {
        /// <summary>
        /// Gets the current <see cref="CompositionBrush"/> instance, if one is in use.
        /// </summary>
        CompositionBrush? Brush { get; }
    }
}
