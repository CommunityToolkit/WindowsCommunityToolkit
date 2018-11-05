// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class RepeaterTransform : Transform
    {
        public RepeaterTransform(
            string name,
            IAnimatableVector3 anchor,
            IAnimatableVector3 position,
            IAnimatableVector3 scalePercent,
            Animatable<double> rotationDegrees,
            Animatable<double> opacityPercent,
            Animatable<double> startOpacityPercent,
            Animatable<double> endOpacityPercent)
            : base(name, anchor, position, scalePercent, rotationDegrees, opacityPercent)
        {
            StartOpacityPercent = startOpacityPercent;
            EndOpacityPercent = endOpacityPercent;
        }
        /// <summary>
        /// Only used by <see cref="Repeater"/>. Defines the opacity of the original shape.
        /// </summary>
        public Animatable<double> StartOpacityPercent { get; }

        /// <summary>
        /// Only used by <see cref="Repeater"/>. Defines the opacity of the last copy of the original shape.
        /// </summary>
        public Animatable<double> EndOpacityPercent { get; }
    }
}
