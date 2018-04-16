// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value
{
    internal class Keyframe<T>
    {
        private readonly LottieComposition _composition;

        public T StartValue { get; }

        public T EndValue { get; }

        public IInterpolator Interpolator { get; }

        public float? StartFrame { get; }

        public float? EndFrame { get; internal set; }

        private float _startProgress = float.MinValue;
        private float _endProgress = float.MinValue;

        // Used by PathKeyframe but it has to be parsed by KeyFrame because we use a JsonReader to
        // deserialzie the data so we have to parse everything in order
        public Vector2? PathCp1 { get; set; }

        public Vector2? PathCp2 { get; set; }

        internal Keyframe(LottieComposition composition, T startValue, T endValue, IInterpolator interpolator, float? startFrame, float? endFrame)
        {
            _composition = composition;
            StartValue = startValue;
            EndValue = endValue;
            Interpolator = interpolator;
            StartFrame = startFrame;
            EndFrame = endFrame;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Keyframe{T}"/> class.
        /// Non-animated value.
        /// </summary>
        /// <param name="value">The static value that the keyframe contains.</param>
        public Keyframe(T value)
        {
            _composition = null;
            StartValue = value;
            EndValue = value;
            Interpolator = null;
            StartFrame = float.MinValue;
            EndFrame = float.MaxValue;
        }

        public virtual float StartProgress
        {
            get
            {
                if (_composition == null)
                {
                    return 0f;
                }

                if (_startProgress == float.MinValue)
                {
                    _startProgress = (StartFrame.Value - _composition.StartFrame) / _composition.DurationFrames;
                }

                return _startProgress;
            }
        }

        public virtual float EndProgress
        {
            get
            {
                if (_composition == null)
                {
                    return 1f;
                }

                if (_endProgress == float.MinValue)
                {
                    if (EndFrame == null)
                    {
                        _endProgress = 1f;
                    }
                    else
                    {
                        var startProgress = StartProgress;
                        var durationFrames = EndFrame.Value - StartFrame.Value;
                        var durationProgress = durationFrames / _composition.DurationFrames;
                        _endProgress = startProgress + durationProgress;
                    }
                }

                return _endProgress;
            }
        }

        public virtual bool Static => Interpolator == null;

        public virtual bool ContainsProgress(float progress)
        {
            return progress >= StartProgress && progress < EndProgress;
        }

        public override string ToString()
        {
            return "Keyframe{" + "startValue=" + StartValue + ", endValue=" + EndValue + ", startFrame=" + StartFrame + ", endFrame=" + EndFrame + ", interpolator=" + Interpolator + '}';
        }
    }
}