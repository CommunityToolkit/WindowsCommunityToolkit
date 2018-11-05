// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class RadialGradientFill : ShapeLayerContent
    {
        public RadialGradientFill(
            string name,
            string matchName,
            Animatable<double> opacityPercent,
            IAnimatableVector3 startPoint,
            IAnimatableVector3 endPoint,
            Animatable<Sequence<GradientStop>> gradientStops,
            Animatable<double> highlightLength,
            Animatable<double> highlightDegrees)
            : base(name, matchName)
        {
            OpacityPercent = opacityPercent;
            StartPoint = startPoint;
            EndPoint = endPoint;
            GradientStops = gradientStops;
            HighlightLength = highlightLength;
            HighlightDegrees = highlightDegrees;
        }


        public IAnimatableVector3 StartPoint { get; }

        public IAnimatableVector3 EndPoint { get; }

        public Animatable<Sequence<GradientStop>> GradientStops { get; }

        public Animatable<double> HighlightLength { get; }

        public Animatable<double> HighlightDegrees { get; }

        public Animatable<double> OpacityPercent { get; }

        public override ShapeContentType ContentType => ShapeContentType.RadialGradientFill;

        public override LottieObjectType ObjectType => LottieObjectType.RadialGradientFill;
    }
}
