// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A <see cref="KeyFrame{TValue,TKeyFrame}"/> type for <see cref="Quaternion"/> animations.
    /// </summary>
    public class QuaternionKeyFrame : KeyFrame<string, Quaternion>
    {
        /// <inheritdoc/>
        public override INormalizedKeyFrameAnimationBuilder<Quaternion> AppendToBuilder(INormalizedKeyFrameAnimationBuilder<Quaternion> builder)
        {
            Vector4 vector = Value!.ToVector4();

            return builder.KeyFrame(
                Key,
                Unsafe.As<Vector4, Quaternion>(ref vector),
                EasingType ?? DefaultEasingType,
                EasingMode ?? DefaultEasingMode);
        }
    }
}
