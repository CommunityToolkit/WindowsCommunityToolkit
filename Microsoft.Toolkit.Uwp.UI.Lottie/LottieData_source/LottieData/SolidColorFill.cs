// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class SolidColorFill : ShapeLayerContent
    {
        public SolidColorFill(
            string name,
            string matchName,
            PathFillType fillType,
            Animatable<Color> color,
            Animatable<double> opacityPercent) 
            : base(name, matchName)
        {
            FillType = fillType;
            Color = color;
            OpacityPercent = opacityPercent;
        }

        public Animatable<Color> Color { get; }

        public Animatable<double> OpacityPercent { get; }


        public PathFillType FillType { get; }

        public override ShapeContentType ContentType => ShapeContentType.SolidColorFill;

        public override LottieObjectType ObjectType => LottieObjectType.SolidColorFill;

        public enum PathFillType
        {
            EvenOdd,
            InverseWinding,
            Winding
        }
    }
}
