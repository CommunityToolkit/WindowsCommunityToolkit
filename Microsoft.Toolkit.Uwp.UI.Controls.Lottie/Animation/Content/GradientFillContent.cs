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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class GradientFillContent : IDrawingContent, IKeyPathElementContent
    {
        /// <summary>
        /// Cache the gradients such that it runs at 30fps.
        /// </summary>
        private const int CacheStepsMs = 32;

        private readonly Dictionary<long, LinearGradient> _linearGradientCache = new Dictionary<long, LinearGradient>();
        private readonly Dictionary<long, RadialGradient> _radialGradientCache = new Dictionary<long, RadialGradient>();
        private readonly Matrix3X3 _shaderMatrix = Matrix3X3.CreateIdentity();
        private readonly Path _path = new Path();
        private readonly Paint _paint = new Paint(Paint.AntiAliasFlag);

        // private Rect _boundsRect;
        private readonly List<IPathContent> _paths = new List<IPathContent>();
        private readonly GradientType _type;
        private readonly IBaseKeyframeAnimation<GradientColor, GradientColor> _colorAnimation;
        private readonly IBaseKeyframeAnimation<int?, int?> _opacityAnimation;
        private readonly IBaseKeyframeAnimation<Vector2?, Vector2?> _startPointAnimation;
        private readonly IBaseKeyframeAnimation<Vector2?, Vector2?> _endPointAnimation;
        private readonly LottieDrawable _lottieDrawable;
        private readonly int _cacheSteps;

        private IBaseKeyframeAnimation<ColorFilter, ColorFilter> _colorFilterAnimation;

        internal GradientFillContent(LottieDrawable lottieDrawable, BaseLayer layer, GradientFill fill)
        {
            Name = fill.Name;
            _lottieDrawable = lottieDrawable;
            _type = fill.GradientType;
            _path.FillType = fill.FillType;
            _cacheSteps = (int)(lottieDrawable.Composition.Duration / CacheStepsMs);

            _colorAnimation = fill.GradientColor.CreateAnimation();
            _colorAnimation.ValueChanged += OnValueChanged;
            layer.AddAnimation(_colorAnimation);

            _opacityAnimation = fill.Opacity.CreateAnimation();
            _opacityAnimation.ValueChanged += OnValueChanged;
            layer.AddAnimation(_opacityAnimation);

            _startPointAnimation = fill.StartPoint.CreateAnimation();
            _startPointAnimation.ValueChanged += OnValueChanged;
            layer.AddAnimation(_startPointAnimation);

            _endPointAnimation = fill.EndPoint.CreateAnimation();
            _endPointAnimation.ValueChanged += OnValueChanged;
            layer.AddAnimation(_endPointAnimation);
        }

        private void OnValueChanged(object sender, EventArgs eventArgs)
        {
            _lottieDrawable.InvalidateSelf();
        }

        public void SetContents(List<IContent> contentsBefore, List<IContent> contentsAfter)
        {
            for (var i = 0; i < contentsAfter.Count; i++)
            {
                if (contentsAfter[i] is IPathContent pathContent)
                {
                    _paths.Add(pathContent);
                }
            }
        }

        public void Draw(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte parentAlpha)
        {
            LottieLog.BeginSection("GradientFillContent.Draw");
            _path.Reset();
            for (var i = 0; i < _paths.Count; i++)
            {
                _path.AddPath(_paths[i].Path, parentMatrix);
            }

            // _path.ComputeBounds(out _boundsRect);
            Shader shader;
            if (_type == GradientType.Linear)
            {
                shader = LinearGradient;
            }
            else
            {
                shader = RadialGradient;
            }

            _shaderMatrix.Set(parentMatrix);
            shader.LocalMatrix = _shaderMatrix;
            _paint.Shader = shader;

            if (_colorFilterAnimation != null)
            {
                _paint.ColorFilter = _colorFilterAnimation.Value;
            }

            var alpha = (byte)(parentAlpha / 255f * _opacityAnimation.Value / 100f * 255);
            _paint.Alpha = alpha;

            canvas.DrawPath(_path, _paint);
            LottieLog.EndSection("GradientFillContent.Draw");
        }

        public void GetBounds(out Rect outBounds, Matrix3X3 parentMatrix)
        {
            _path.Reset();
            for (var i = 0; i < _paths.Count; i++)
            {
                _path.AddPath(_paths[i].Path, parentMatrix);
            }

            _path.ComputeBounds(out outBounds);

            // Add padding to account for rounding errors.
            RectExt.Set(ref outBounds, outBounds.Left - 1, outBounds.Top - 1, outBounds.Right + 1, outBounds.Bottom + 1);
        }

        public string Name { get; }

        private LinearGradient LinearGradient
        {
            get
            {
                var gradientHash = GradientHash;
                if (_linearGradientCache.TryGetValue(gradientHash, out LinearGradient gradient))
                {
                    return gradient;
                }

                var startPoint = _startPointAnimation.Value;
                var endPoint = _endPointAnimation.Value;
                var gradientColor = _colorAnimation.Value;
                var colors = gradientColor.Colors;
                var positions = gradientColor.Positions;
                gradient = new LinearGradient(startPoint.Value.X, startPoint.Value.Y, endPoint.Value.X, endPoint.Value.Y, colors, positions);
                _linearGradientCache.Add(gradientHash, gradient);
                return gradient;
            }
        }

        private RadialGradient RadialGradient
        {
            get
            {
                var gradientHash = GradientHash;
                if (_radialGradientCache.TryGetValue(gradientHash, out RadialGradient gradient))
                {
                    return gradient;
                }

                var startPoint = _startPointAnimation.Value;
                var endPoint = _endPointAnimation.Value;
                var gradientColor = _colorAnimation.Value;
                var colors = gradientColor.Colors;
                var positions = gradientColor.Positions;
                var x0 = startPoint.Value.X;
                var y0 = startPoint.Value.Y;
                var x1 = endPoint.Value.X;
                var y1 = endPoint.Value.Y;
                var r = (float)MathExt.Hypot(x1 - x0, y1 - y0);
                gradient = new RadialGradient(x0, y0, r, colors, positions);
                _radialGradientCache.Add(gradientHash, gradient);
                return gradient;
            }
        }

        private int GradientHash
        {
            get
            {
                var startPointProgress = (int)Math.Round(_startPointAnimation.Progress * _cacheSteps);
                var endPointProgress = (int)Math.Round(_endPointAnimation.Progress * _cacheSteps);
                var colorProgress = (int)Math.Round(_colorAnimation.Progress * _cacheSteps);
                var hash = 17;
                if (startPointProgress != 0)
                {
                    hash = hash * 31 * startPointProgress;
                }

                if (endPointProgress != 0)
                {
                    hash = hash * 31 * endPointProgress;
                }

                if (colorProgress != 0)
                {
                    hash = hash * 31 * colorProgress;
                }

                return hash;
            }
        }

        public void ResolveKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath)
        {
            MiscUtils.ResolveKeyPath(keyPath, depth, accumulator, currentPartialKeyPath, this);
        }

        public void AddValueCallback<T>(LottieProperty property, ILottieValueCallback<T> callback)
        {
            if (property == LottieProperty.ColorFilter)
            {
                if (callback == null)
                {
                    _colorFilterAnimation = null;
                }
                else
                {
                    _colorFilterAnimation = new ValueCallbackKeyframeAnimation<ColorFilter, ColorFilter>((ILottieValueCallback<ColorFilter>)callback);
                }
            }
        }
    }
}