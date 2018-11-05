// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    /// <summary>
    /// A value that may be animated.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
#if !WINDOWS_UWP
    public
#endif
    class Animatable<T> : IAnimatableValue<T> where T : IEquatable<T>
    {
        internal static readonly IEnumerable<KeyFrame<T>> s_emptyKeyFrames = new KeyFrame<T>[0];
        readonly KeyFrame<T>[] _keyFrames;

        /// <summary>
        /// Constructs a non-animated value.
        /// </summary>
        public Animatable(T value, int? propertyIndex)
            : this(value, s_emptyKeyFrames, propertyIndex)
        { }

        /// <summary>
        /// Constructs a value animated by the given key frames.
        /// </summary>
        public Animatable(T initialValue, IEnumerable<KeyFrame<T>> keyFrames, int? propertyIndex)
        {
            _keyFrames = keyFrames.ToArray();
            InitialValue = initialValue;
            PropertyIndex = propertyIndex;

            Debug.Assert(initialValue != null);
            Debug.Assert(keyFrames.All(kf => kf != null));
        }

        /// <summary>
        /// The initial value.
        /// </summary>
        public T InitialValue { get; }

        /// <summary>
        /// The keyframes that describe how the value should be animated.
        /// </summary>
        public IEnumerable<KeyFrame<T>> KeyFrames => _keyFrames;

        /// <summary>
        /// Property index used for expressions.
        /// </summary>
        public int? PropertyIndex { get; }

        /// <summary>
        /// Returns true iff this value has any key frames.
        /// </summary>
        public bool IsAnimated => _keyFrames.Length > 0;

        /// <summary>
        /// Returns true if this value is always equal to the given value.
        /// </summary>
        public bool AlwaysEquals(T value) => !IsAnimated && value.Equals(InitialValue);

        // Not a great hash code because it ignore the KeyFrames, but quick.
        public override int GetHashCode() => InitialValue.GetHashCode();

        public override string ToString() =>
            IsAnimated
                ? string.Join(" -> ", KeyFrames.Select(kf => kf.Value.ToString()))
                : InitialValue.ToString();
    }
}
