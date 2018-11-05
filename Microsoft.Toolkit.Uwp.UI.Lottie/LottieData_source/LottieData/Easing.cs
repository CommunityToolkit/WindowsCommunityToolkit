// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class Easing : IEquatable<Easing>
    {
        protected private Easing() { }

        public abstract EasingType Type { get; }

        public bool Equals(Easing other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            if (Type != other.Type)
            {
                return false;
            }
            switch (Type)
            {
                case EasingType.Hold:
                case EasingType.Linear:
                    // Linear and hold easings have no parameters, so they're all equivalent to each other.
                    return true;
                case EasingType.CubicBezier:
                    var xCb = (CubicBezierEasing)this;
                    var yCb = (CubicBezierEasing)other;
                    if (!xCb.ControlPoint1.Equals(yCb.ControlPoint1))
                    {
                        return false;
                    }
                    return true;
                default:
                    throw new InvalidOperationException();
            }
        }

        public enum EasingType
        {
            Linear,
            CubicBezier,
            Hold,
        }

    }
}
