// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// Mask class representing the Lottie mask properties
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class Mask
    {
        public Mask(
            bool inverted,
            string name,
            Animatable<Sequence<BezierSegment>> points,
            Animatable<double> opacity,
            MaskMode mode
        )
        {
            Inverted = inverted;
            Name = name;
            Points = points;
            Opacity = opacity;
            Mode = mode;
        }

        public bool Inverted { get; }

        public string Name { get; }

        public Animatable<Sequence<BezierSegment>> Points { get; }

        public Animatable<double> Opacity { get; }

        public MaskMode Mode { get; }

        public enum MaskMode
        {
            None = 0,
            Additive,
            Darken,
            Difference,
            Intersect,
            Lighten,
            Subtract,
        }
    }
}
