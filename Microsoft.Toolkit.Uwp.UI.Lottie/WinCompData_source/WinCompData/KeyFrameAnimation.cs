// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData
{
#if !WINDOWS_UWP
    public
#endif
    abstract class KeyFrameAnimation_ : CompositionAnimation
    {
        public TimeSpan Duration { get; set; }

        public abstract int KeyFrameCount { get; }

        protected private KeyFrameAnimation_(KeyFrameAnimation_ other) : base(other) { }
    }

#if !WINDOWS_UWP
    public
#endif
    abstract class KeyFrameAnimation<T> : KeyFrameAnimation_
    {
        readonly Dictionary<float, KeyFrame> _keyFrames = new Dictionary<float, KeyFrame>();

        protected private KeyFrameAnimation(KeyFrameAnimation<T> other) : base(other)
        {
            if (other != null)
            {
                CopyStateFrom(other);
            }
        }

        public void InsertExpressionKeyFrame(float progress, string expression, CompositionEasingFunction easing)
        {
            if (progress < 0 || progress > 1)
            {
                throw new ArgumentException($"Progress must be >=0 and <=1. Value: {progress}");
            }
            _keyFrames.Add(progress, new ExpressionKeyFrame { Progress = progress, Expression = expression, Easing = easing });
        }

        public void InsertKeyFrame(float progress, T value, CompositionEasingFunction easing)
        {
            if (progress < 0 || progress > 1)
            {
                throw new ArgumentException($"Progress must be >=0 and <=1. Value: {progress}");
            }

            _keyFrames.Add(progress, new ValueKeyFrame { Progress = progress, Value = value, Easing = easing });
        }

        public IEnumerable<KeyFrame> KeyFrames => _keyFrames.Values.OrderBy(kf => kf.Progress);

        public override int KeyFrameCount => _keyFrames.Count;

        void CopyStateFrom(KeyFrameAnimation<T> other)
        {
            _keyFrames.Clear();
            foreach (var pair in other._keyFrames)
            {
                _keyFrames.Add(pair.Key, pair.Value);
            }
            Duration = other.Duration;
            Target = other.Target;
        }

        public enum KeyFrameType
        {
            Expression,
            Value,
        }

        public abstract class KeyFrame 
        {
            protected private KeyFrame() { }

            public float Progress { get; internal set; }
            public CompositionEasingFunction Easing { get; internal set; }
            public abstract KeyFrameType Type { get; }

        }

        public sealed class ValueKeyFrame : KeyFrame
        {
            public T Value { get; internal set; }

            public override KeyFrameType Type => KeyFrameType.Value;
        }

        public sealed class ExpressionKeyFrame : KeyFrame
        {
            public string Expression { get; internal set; }

            public override KeyFrameType Type => KeyFrameType.Expression;

        }

    }
}
