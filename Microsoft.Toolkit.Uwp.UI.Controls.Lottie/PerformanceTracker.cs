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
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Provides a mechanism for tracking the performance of a Lottie Animation
    /// </summary>
    public class PerformanceTracker
    {
        /// <summary>
        /// Provides data for the FrameRendered event.
        /// </summary>
        public class FrameRenderedEventArgs : EventArgs
        {
            internal FrameRenderedEventArgs(float renderTimeMs)
            {
                RenderTimeMs = renderTimeMs;
            }

            /// <summary>
            /// Gets the time in milliseconds that this frame took to render
            /// </summary>
            public float RenderTimeMs { get; }
        }

        private bool _enabled;

        /// <summary>
        /// Occurs when a a new frame is rendered on the Lottie Animation
        /// </summary>
        public event EventHandler<FrameRenderedEventArgs> FrameRendered;

        private readonly Dictionary<string, MeanCalculator> _layerRenderTimes = new Dictionary<string, MeanCalculator>();
        private readonly IComparer<Tuple<string, float?>> _floatComparator = new ComparatorAnonymousInnerClass();

        private class ComparatorAnonymousInnerClass : IComparer<Tuple<string, float?>>
        {
            public int Compare(Tuple<string, float?> o1, Tuple<string, float?> o2)
            {
                var r1 = o1.Item2;
                var r2 = o2.Item2;
                if (r2 > r1)
                {
                    return 1;
                }

                if (r1 > r2)
                {
                    return -1;
                }

                return 0;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the performance tracker is enabled or not
        /// </summary>
        public virtual bool Enabled
        {
            set => _enabled = value;
        }

        /// <summary>
        /// Method called internally to track the time that a specific layer took to render
        /// </summary>
        /// <param name="layerName">The name of the layer being tracked</param>
        /// <param name="millis">The time that this layer took to render</param>
        public virtual void RecordRenderTime(string layerName, float millis)
        {
            if (!_enabled)
            {
                return;
            }

            if (!_layerRenderTimes.TryGetValue(layerName, out var meanCalculator))
            {
                meanCalculator = new MeanCalculator();
                _layerRenderTimes[layerName] = meanCalculator;
            }

            meanCalculator.Add(millis);
            if (layerName.Equals("__container"))
            {
                OnFrameRendered(new FrameRenderedEventArgs(millis));
            }
        }

        /// <summary>
        /// Clears all the rendering times
        /// </summary>
        public virtual void ClearRenderTimes()
        {
            _layerRenderTimes.Clear();
        }

        /// <summary>
        /// Logs all the rendering times into the Debug.WriteLine inder the <see cref="LottieLog.Tag"/> category
        /// </summary>
        public virtual void LogRenderTimes()
        {
            if (!_enabled)
            {
                return;
            }

            var sortedRenderTimes = SortedRenderTimes;
            Debug.WriteLine("Render times:", LottieLog.Tag);
            for (var i = 0; i < sortedRenderTimes.Count; i++)
            {
                var layer = sortedRenderTimes[i];
                Debug.WriteLine(string.Format("\t\t{0,30}:{1:F2}", layer.Item1, layer.Item2), LottieLog.Tag);
            }
        }

        /// <summary>
        /// Gets the render times of all tracked render times logs ordered by the means of each layer
        /// </summary>
        public virtual List<Tuple<string, float?>> SortedRenderTimes
        {
            get
            {
                if (!_enabled)
                {
                    return new List<Tuple<string, float?>>();
                }

                var sortedRenderTimes = new List<Tuple<string, float?>>(_layerRenderTimes.Count);
                foreach (var e in _layerRenderTimes.SetOfKeyValuePairs())
                {
                    sortedRenderTimes.Add(new Tuple<string, float?>(e.Key, e.Value.Mean));
                }

                sortedRenderTimes.Sort(_floatComparator);
                return sortedRenderTimes;
            }
        }

        /// <summary>
        /// Invoked whenever a frame is rendered
        /// </summary>
        /// <param name="e">Provides rendering information about the frame that was rendered</param>
        protected virtual void OnFrameRendered(FrameRenderedEventArgs e)
        {
            FrameRendered?.Invoke(this, e);
        }
    }
}