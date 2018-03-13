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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class ShapeContent : IPathContent
    {
        private readonly Path _path = new Path();

        private readonly LottieDrawable _lottieDrawable;
        private readonly IBaseKeyframeAnimation<ShapeData, Path> _shapeAnimation;

        private bool _isPathValid;
        private TrimPathContent _trimPath;

        internal ShapeContent(LottieDrawable lottieDrawable, BaseLayer layer, ShapePath shape)
        {
            Name = shape.Name;
            _lottieDrawable = lottieDrawable;
            _shapeAnimation = shape.GetShapePath().CreateAnimation();
            layer.AddAnimation(_shapeAnimation);
            _shapeAnimation.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged(object sender, EventArgs eventArgs)
        {
            Invalidate();
        }

        private void Invalidate()
        {
            _isPathValid = false;
            _lottieDrawable.InvalidateSelf();
        }

        public void SetContents(List<IContent> contentsBefore, List<IContent> contentsAfter)
        {
            for (var i = 0; i < contentsBefore.Count; i++)
            {
                if (contentsBefore[i] is TrimPathContent trimPathContent && trimPathContent.Type == ShapeTrimPath.Type.Simultaneously)
                {
                    // Trim path individually will be handled by the stroke where paths are combined.
                    _trimPath = trimPathContent;
                    _trimPath.ValueChanged += OnValueChanged;
                }
            }
        }

        public Path Path
        {
            get
            {
                if (_isPathValid)
                {
                    return _path;
                }

                _path.Reset();

                _path.Set(_shapeAnimation.Value);
                _path.FillType = PathFillType.EvenOdd;

                Utils.Utils.ApplyTrimPathIfNeeded(_path, _trimPath);

                _isPathValid = true;
                return _path;
            }
        }

        public string Name { get; }
    }
}