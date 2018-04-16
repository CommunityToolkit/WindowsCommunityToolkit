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

using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Parser
{
    internal static class KeyframeParser
    {
        /// <summary>
        /// Some animations get exported with insane cp values in the tens of thousands.
        /// PathInterpolator fails to create the interpolator in those cases and hangs.
        /// Clamping the cp helps prevent that.
        /// </summary>
        private const float MaxCpValue = 100;
        private static readonly IInterpolator LinearInterpolator = new LinearInterpolator();

        private static readonly object Lock = new object();
        private static Dictionary<int, WeakReference<IInterpolator>> _pathInterpolatorCache;

        // https://github.com/airbnb/lottie-android/issues/464
        private static Dictionary<int, WeakReference<IInterpolator>> PathInterpolatorCache()
        {
            return _pathInterpolatorCache ?? (_pathInterpolatorCache = new Dictionary<int, WeakReference<IInterpolator>>());
        }

        private static bool GetInterpolator(int hash, out WeakReference<IInterpolator> interpolatorRef)
        {
            // This must be synchronized because get and put isn't thread safe because
            // SparseArrayCompat has to create new sized arrays sometimes.
            lock (Lock)
            {
                return PathInterpolatorCache().TryGetValue(hash, out interpolatorRef);
            }
        }

        private static void PutInterpolator(int hash, WeakReference<IInterpolator> interpolator)
        {
            // This must be synchronized because get and put isn't thread safe because
            // SparseArrayCompat has to create new sized arrays sometimes.
            lock (Lock)
            {
                _pathInterpolatorCache[hash] = interpolator;
            }
        }

        internal static Keyframe<T> Parse<T>(JsonReader reader, LottieComposition composition, float scale, IValueParser<T> valueParser, bool animated)
        {
            if (animated)
            {
                return ParseKeyframe(composition, reader, scale, valueParser);
            }

            return ParseStaticValue(reader, scale, valueParser);
        }

        // beginObject will already be called on the keyframe so it can be differentiated with a non animated value.
        private static Keyframe<T> ParseKeyframe<T>(LottieComposition composition, JsonReader reader, float scale, IValueParser<T> valueParser)
        {
            Vector2? cp1 = null;
            Vector2? cp2 = null;
            float startFrame = 0;
            T startValue = default(T);
            T endValue = default(T);
            bool hold = false;
            IInterpolator interpolator;

            // Only used by PathKeyframe
            Vector2? pathCp1 = null;
            Vector2? pathCp2 = null;

            reader.BeginObject();
            while (reader.HasNext())
            {
                switch (reader.NextName())
                {
                    case "t":
                        startFrame = reader.NextDouble();
                        break;
                    case "s":
                        startValue = valueParser.Parse(reader, scale);
                        break;
                    case "e":
                        endValue = valueParser.Parse(reader, scale);
                        break;
                    case "o":
                        cp1 = JsonUtils.JsonToPoint(reader, scale);
                        break;
                    case "i":
                        cp2 = JsonUtils.JsonToPoint(reader, scale);
                        break;
                    case "h":
                        hold = reader.NextInt() == 1;
                        break;
                    case "to":
                        pathCp1 = JsonUtils.JsonToPoint(reader, scale);
                        break;
                    case "ti":
                        pathCp2 = JsonUtils.JsonToPoint(reader, scale);
                        break;
                    default:
                        reader.SkipValue();
                        break;
                }
            }

            reader.EndObject();

            if (hold)
            {
                endValue = startValue;

                // TODO: create a HoldInterpolator so progress changes don't invalidate.
                interpolator = LinearInterpolator;
            }
            else if (cp1 != null && cp2 != null)
            {
                cp1 = new Vector2(
                    MiscUtils.Clamp(cp1.Value.X, -scale, scale),
                    MiscUtils.Clamp(cp1.Value.Y, -MaxCpValue, MaxCpValue));
                cp2 = new Vector2(
                    MiscUtils.Clamp(cp2.Value.X, -scale, scale),
                    MiscUtils.Clamp(cp2.Value.Y, -MaxCpValue, MaxCpValue));

                int hash = Utils.Utils.HashFor(cp1.Value.X, cp1.Value.Y, cp2.Value.X, cp2.Value.Y);
                if (GetInterpolator(hash, out var interpolatorRef) == false ||
                    interpolatorRef.TryGetTarget(out interpolator) == false)
                {
                    interpolator = new PathInterpolator(cp1.Value.X / scale, cp1.Value.Y / scale, cp2.Value.X / scale, cp2.Value.Y / scale);
                    try
                    {
                        PutInterpolator(hash, new WeakReference<IInterpolator>(interpolator));
                    }
                    catch
                    {
                        // It is not clear why but SparseArrayCompat sometimes fails with this:
                        //     https://github.com/airbnb/lottie-android/issues/452
                        // Because this is not a critical operation, we can safely just ignore it.
                        // I was unable to repro this to attempt a proper fix.
                    }
                }
            }
            else
            {
                interpolator = LinearInterpolator;
            }

            var keyframe = new Keyframe<T>(composition, startValue, endValue, interpolator, startFrame, null)
            {
                PathCp1 = pathCp1,
                PathCp2 = pathCp2
            };
            return keyframe;
        }

        private static Keyframe<T> ParseStaticValue<T>(JsonReader reader, float scale, IValueParser<T> valueParser)
        {
            T value = valueParser.Parse(reader, scale);
            return new Keyframe<T>(value);
        }
    }
}
