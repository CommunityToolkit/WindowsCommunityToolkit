// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class LinearGradientFill : ShapeLayerContent
    {
        public LinearGradientFill(
            string name,
            string matchName,
            Animatable<double> opacityPercent,
            Animatable<Vector2> startPoint,
            Animatable<Vector2> endPoint,
            Animatable<Sequence<GradientStop>> gradientStops)
            : base(name, matchName)
        {
            OpacityPercent = opacityPercent;
            StartPoint = startPoint;
            EndPoint = endPoint;
            GradientStops = gradientStops;
        }

        public Animatable<Vector2> StartPoint { get; }

        public Animatable<Vector2> EndPoint { get; }

        public Animatable<double> OpacityPercent { get; }

        public Animatable<Sequence<GradientStop>> GradientStops { get; }

        public override ShapeContentType ContentType => ShapeContentType.LinearGradientFill;

        public override LottieObjectType ObjectType => LottieObjectType.LinearGradientFill;
    }
}
