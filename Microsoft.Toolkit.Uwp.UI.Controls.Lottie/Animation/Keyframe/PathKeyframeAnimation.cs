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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe
{
    internal class PathKeyframeAnimation : KeyframeAnimation<Vector2?>, IDisposable
    {
        private PathKeyframe _pathMeasureKeyframe;
        private PathMeasure _pathMeasure;

        internal PathKeyframeAnimation(List<Keyframe<Vector2?>> keyframes)
            : base(keyframes)
        {
        }

        public override Vector2? GetValue(Keyframe<Vector2?> keyframe, float keyframeProgress)
        {
            var pathKeyframe = (PathKeyframe)keyframe;
            var path = pathKeyframe.Path;
            if (path == null || path.Contours.Count == 0)
            {
                return keyframe.StartValue;
            }

            if (ValueCallback != null)
            {
                return ValueCallback.GetValueInternal(pathKeyframe.StartFrame.Value, pathKeyframe.EndFrame.Value, pathKeyframe.StartValue, pathKeyframe.EndValue, LinearCurrentKeyframeProgress, keyframeProgress, Progress);
            }

            if (_pathMeasureKeyframe != pathKeyframe)
            {
                _pathMeasure?.Dispose();
                _pathMeasure = new PathMeasure(path);
                _pathMeasureKeyframe = pathKeyframe;
            }

            return _pathMeasure.GetPosTan(keyframeProgress * _pathMeasure.Length);
        }

        private void Dispose(bool disposing)
        {
            if (_pathMeasure != null)
            {
                _pathMeasure.Dispose(disposing);
                _pathMeasure = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PathKeyframeAnimation()
        {
            Dispose(false);
        }
    }
}