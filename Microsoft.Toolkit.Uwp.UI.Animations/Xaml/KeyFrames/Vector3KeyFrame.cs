// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A <see cref="KeyFrame{TValue,TKeyFrame}"/> type for <see cref="Vector3"/> animations.
    /// </summary>
    public class Vector3KeyFrame : KeyFrame<string, Vector3>
    {
        /// <inheritdoc/>
        public override INormalizedKeyFrameAnimationBuilder<Vector3> AppendToBuilder(INormalizedKeyFrameAnimationBuilder<Vector3> builder)
        {
            return builder.KeyFrame(Key, Value!.ToVector3(), EasingType ?? DefaultEasingType, EasingMode ?? DefaultEasingMode);
        }
    }
}
