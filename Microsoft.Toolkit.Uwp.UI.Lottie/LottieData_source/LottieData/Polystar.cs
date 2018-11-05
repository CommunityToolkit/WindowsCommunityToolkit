// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Polystar : ShapeLayerContent
    {
        public Polystar(
            string name,
            string matchName,
            bool direction,
            PolyStarType starType,
            Animatable<double> points,
            IAnimatableVector3 position,
            Animatable<double> rotation,
            Animatable<double> innerRadius,
            Animatable<double> outerRadius,
            Animatable<double> innerRoundedness,
            Animatable<double> outerRoundedness)
            : base(name, matchName)
        {
            Direction = direction;
            StarType = starType;
            Points = points;
            Position = position;
            Rotation = rotation;
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
            InnerRoundedness = innerRoundedness;
            OuterRoundedness = outerRoundedness;
        }

        internal bool Direction { get; }
        internal PolyStarType StarType { get; }

        internal Animatable<double> Points { get; }

        internal IAnimatableVector3 Position { get; }

        internal Animatable<double> Rotation { get; }

        internal Animatable<double> InnerRadius { get; }

        internal Animatable<double> OuterRadius { get; }

        internal Animatable<double> InnerRoundedness { get; }

        internal Animatable<double> OuterRoundedness { get; }

        public override ShapeContentType ContentType => ShapeContentType.Polystar;

        public override LottieObjectType ObjectType => LottieObjectType.Polystar;

        public enum PolyStarType
        {
            Star,
            Polygon,
        }
    }
}
