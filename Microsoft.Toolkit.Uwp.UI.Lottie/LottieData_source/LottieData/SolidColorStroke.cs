// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class SolidColorStroke : ShapeLayerContent
    {
        public SolidColorStroke(
            string name,
            string matchName,
            Animatable<double> dashOffset,
            IEnumerable<double> dashPattern,
            Animatable<Color> color,
            Animatable<double> opacityPercent,
            Animatable<double> thickness,
            LineCapType capType,
            LineJoinType joinType,
            double miterLimit)
            : base(name, matchName)
        {
            DashOffset = dashOffset;
            DashPattern = dashPattern;
            Color = color;
            OpacityPercent = opacityPercent;
            Thickness = thickness;
            CapType = capType;
            JoinType = joinType;
            MiterLimit = miterLimit;
        }

        public Animatable<Color> Color { get; }

        public Animatable<double> OpacityPercent { get; }

        public Animatable<double> Thickness { get; }

        public IEnumerable<double> DashPattern { get; }

        public Animatable<double> DashOffset { get; }

        public LineCapType CapType { get; }

        public LineJoinType JoinType { get; }

        public double MiterLimit { get; }

        public override ShapeContentType ContentType => ShapeContentType.SolidColorStroke;

        public override LottieObjectType ObjectType => LottieObjectType.SolidColorStroke;

        public enum LineCapType
        {
            Butt,
            Round,
            Projected,
        }

        public enum LineJoinType
        {
            Miter,
            Round,
            Bevel,
        }
    }
}
