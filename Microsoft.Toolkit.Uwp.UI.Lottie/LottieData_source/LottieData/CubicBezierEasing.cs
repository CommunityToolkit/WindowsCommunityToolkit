// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    sealed class CubicBezierEasing : Easing, IEquatable<CubicBezierEasing>
    {
        public CubicBezierEasing(Vector3 controlPoint1, Vector3 controlPoint2)
        {
            ControlPoint1 = controlPoint1;
            ControlPoint2 = controlPoint2;
        }

        public Vector3 ControlPoint1 { get; }
        public Vector3 ControlPoint2 { get; }

        public override EasingType Type => EasingType.CubicBezier;

        public override bool Equals(object obj) => Equals(obj as CubicBezierEasing);

        public bool Equals(CubicBezierEasing other) =>
               ReferenceEquals(this, other) ||
                (other != null &&
                ControlPoint1.Equals(other.ControlPoint1) &&
                ControlPoint2.Equals(other.ControlPoint2));

        public override int GetHashCode() => ControlPoint1.GetHashCode() ^ ControlPoint2.GetHashCode();

        public override string ToString() => nameof(CubicBezierEasing);
    }
}
