// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// Describes a value at a particular point in time, and an optional easing function to
    /// interpolate from the previous value.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class KeyFrame<T> : IEquatable<KeyFrame<T>> where T : IEquatable<T>
    {
        public KeyFrame(double frame, T value, Vector3 spatialControlPoint1, Vector3 spatialControlPoint2, Easing easing)
        {
            Frame = frame;
            Value = value;
            SpatialControlPoint1 = spatialControlPoint1;
            SpatialControlPoint2 = spatialControlPoint2;
            Easing = easing;
        }

        /// <summary>
        /// The value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// The frame at which the animation should have the <see cref="Value"/>.
        /// </summary>
        public double Frame { get; }

        /// <summary>
        /// Only valid on Vector3 keyframes. Defines the path the animation follows.
        /// </summary>
        public Vector3 SpatialControlPoint1 { get; }

        /// <summary>
        /// Only valid on Vector3 keyframes. Defines the path the animation follows.
        /// </summary>
        public Vector3 SpatialControlPoint2 { get; }

        /// <summary>
        /// The easing function used to interpolate from the previous value.
        /// </summary>
        public Easing Easing { get; }

        public bool Equals(KeyFrame<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }

            if (!Value.Equals(other.Value))
            {
                return false;
            }
            if (Frame != other.Frame)
            {
                return false;
            }

            if (!Equals(Easing, other.Easing))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode() =>  Value.GetHashCode() ^ Frame.GetHashCode() ^ Easing.GetHashCode();

        public override string ToString() => Easing == null ? $"{Value} @{Frame}" : $"{Value} @{Frame} using {Easing}";
    }
}
