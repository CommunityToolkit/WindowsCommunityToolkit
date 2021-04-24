// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A rotation animation working on a <see cref="SurfaceBrushFactory"/>.
    /// </summary>
    public sealed class RotationAngleInDegreesBrushAnimation : EffectAnimation<SurfaceBrushFactory, double?, double>
    {
        /// <inheritdoc/>
        protected override string ExplicitTarget => nameof(CompositionSurfaceBrush.RotationAngleInDegrees);

        /// <inheritdoc/>
        protected override (double?, double?) GetParsedValues()
        {
            return (To, From);
        }
    }
}
