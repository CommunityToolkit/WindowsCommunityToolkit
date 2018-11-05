// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.


namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    enum LottieObjectType
    {
        Ellipse,
        ImageLayer,
        LinearGradientFill,
        LinearGradientStroke,
        LottieComposition,
        Marker,
        MergePaths,
        NullLayer,
        Polystar,
        PreCompLayer,
        RadialGradientFill,
        RadialGradientStroke,
        Rectangle,
        Repeater,
        RoundedCorner,
        Shape,
        ShapeGroup,
        ShapeLayer,
        SolidColorFill,
        SolidColorStroke,
        SolidLayer,
        TextLayer,
        Transform,
        TrimPath,
    }
}
