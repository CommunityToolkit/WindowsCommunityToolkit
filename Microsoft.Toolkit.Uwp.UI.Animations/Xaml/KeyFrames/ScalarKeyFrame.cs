// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A <see cref="KeyFrame{TValue,TKeyFrame}"/> type for scalar animations.
    /// </summary>
    public class ScalarKeyFrame : KeyFrame<double, double>
    {
        /// <inheritdoc/>
        public override INormalizedKeyFrameAnimationBuilder<double> AppendToBuilder(INormalizedKeyFrameAnimationBuilder<double> builder)
        {
            return builder.KeyFrame(Key, Value!, EasingType ?? DefaultEasingType, EasingMode ?? DefaultEasingMode);
        }
    }
}
