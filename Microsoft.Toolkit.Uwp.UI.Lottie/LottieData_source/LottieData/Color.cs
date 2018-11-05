// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class Color : IEquatable<Color>
    {
        Color(double a, double r, double g, double b) { A = a; R = r; G = g; B = b; }

        public static Color FromArgb(double a, double r, double g, double b)
        {
            return new Color(a, r, g, b);
        }

        public static Color Black { get; } = new Color(1, 0, 0, 0);
        public double A { get; }
        public double R { get; }
        public double G { get; }
        public double B { get; }

        public override bool Equals(object obj) => obj is Color && Equals((Color)obj);
        public bool Equals(Color other) => A == other.A && R == other.R && G == other.G && B == other.B;
        public override int GetHashCode() => (A * R * G * B).GetHashCode();

        public override string ToString() => $"#{ToHex(A)}{ToHex(R)}{ToHex(G)}{ToHex(B)}";

        static string ToHex(double value) => ((byte)(value * 255)).ToString("X2");

    }
}
