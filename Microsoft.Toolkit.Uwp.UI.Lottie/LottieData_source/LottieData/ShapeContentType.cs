// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// Types of <see cref="ShapeLayerContent"/> objects.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    enum ShapeContentType
    {
        Ellipse,
        Group,
        LinearGradientFill,
        LinearGradientStroke,
        MergePaths,
        Path,
        Polystar,
        RadialGradientFill,
        RadialGradientStroke,
        Rectangle,
        Repeater,
        RoundedCorner,
        SolidColorFill,
        SolidColorStroke,
        Transform,
        TrimPath,
    }
}
