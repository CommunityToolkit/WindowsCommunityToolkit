// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Shape : ShapeLayerContent
    {
        public Shape(
            string name,
            string matchName,
            bool direction,
            Animatable<Sequence<BezierSegment>> geometry)
            : base(name, matchName)
        {
            Direction = direction;
            PathData = geometry;
        }

        public bool Direction { get; }

        public Animatable<Sequence<BezierSegment>> PathData { get; }

        public override ShapeContentType ContentType => ShapeContentType.Path;

        public override LottieObjectType ObjectType => LottieObjectType.Shape;
    }
}

