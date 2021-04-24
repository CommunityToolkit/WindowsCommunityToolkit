// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Media;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// An anchor point animation working on a <see cref="SurfaceBrushFactory"/>.
    /// </summary>
    public sealed class AnchorPointBrushAnimation : EffectAnimation<SurfaceBrushFactory, string, Vector2>
    {
        /// <inheritdoc/>
        protected override string ExplicitTarget => nameof(CompositionSurfaceBrush.AnchorPoint);

        /// <inheritdoc/>
        protected override (Vector2?, Vector2?) GetParsedValues()
        {
            return (To?.ToVector2(), From?.ToVector2());
        }
    }
}
