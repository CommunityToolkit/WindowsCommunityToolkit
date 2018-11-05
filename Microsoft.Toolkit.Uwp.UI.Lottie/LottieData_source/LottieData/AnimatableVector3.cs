// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// An animatable Vector3 value expressed as a single animatable Vector3 value.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class AnimatableVector3 : Animatable<Vector3>, IAnimatableVector3
    {
        public AnimatableVector3(Vector3 initialValue, int? propertyIndex)
            : this(initialValue, s_emptyKeyFrames, propertyIndex) { }

        public AnimatableVector3(Vector3 initialValue, IEnumerable<KeyFrame<Vector3>> keyframes, int? propertyIndex)
            : base(initialValue, keyframes, propertyIndex)
        {
        }

        public AnimatableVector3Type Type => AnimatableVector3Type.Vector3;
    }
}


