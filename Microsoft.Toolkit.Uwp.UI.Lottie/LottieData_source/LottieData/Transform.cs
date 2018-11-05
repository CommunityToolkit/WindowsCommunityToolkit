// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    class Transform : ShapeLayerContent
    {
        public Transform(
            string name,
            IAnimatableVector3 anchor,
            IAnimatableVector3 position,
            IAnimatableVector3 scalePercent,
            Animatable<double> rotationDegrees,
            Animatable<double> opacityPercent)
            : base(name, "")
        {
            Anchor = anchor;
            Position = position;
            ScalePercent = scalePercent;
            RotationDegrees = rotationDegrees;
            OpacityPercent = opacityPercent;
        }

        /// <summary>
        /// The point around which scaling and rotation is performed, and from which the position is offset.
        /// </summary>
        public IAnimatableVector3 Anchor { get; }

        /// <summary>
        /// The position, specified as the offset from the <see cref="Anchor"/>.
        /// </summary>
        public IAnimatableVector3 Position { get; }

        public IAnimatableVector3 ScalePercent { get; }

        public Animatable<double> RotationDegrees { get; }

        public Animatable<double> OpacityPercent { get; }

        public bool IsAnimated => Anchor.IsAnimated || Position.IsAnimated || ScalePercent.IsAnimated || RotationDegrees.IsAnimated || OpacityPercent.IsAnimated;

        public override ShapeContentType ContentType => ShapeContentType.Transform;

        public override LottieObjectType ObjectType => LottieObjectType.Transform;
    }
}
