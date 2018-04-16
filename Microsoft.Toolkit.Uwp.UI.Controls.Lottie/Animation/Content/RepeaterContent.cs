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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class RepeaterContent : IDrawingContent, IPathContent, IGreedyContent, IKeyPathElementContent
    {
        private readonly Path _path = new Path();
        private readonly LottieDrawable _lottieDrawable;
        private readonly BaseLayer _layer;
        private readonly IBaseKeyframeAnimation<float?, float?> _copies;
        private readonly IBaseKeyframeAnimation<float?, float?> _offset;
        private readonly TransformKeyframeAnimation _transform;

        private Matrix3X3 _matrix = Matrix3X3.CreateIdentity();
        private ContentGroup _contentGroup;

        internal RepeaterContent(LottieDrawable lottieDrawable, BaseLayer layer, Repeater repeater)
        {
            _lottieDrawable = lottieDrawable;
            _layer = layer;
            Name = repeater.Name;
            _copies = repeater.Copies.CreateAnimation();
            layer.AddAnimation(_copies);
            _copies.ValueChanged += OnValueChanged;

            _offset = repeater.Offset.CreateAnimation();
            layer.AddAnimation(_offset);
            _offset.ValueChanged += OnValueChanged;

            _transform = repeater.Transform.CreateAnimation();
            _transform.AddAnimationsToLayer(layer);
            _transform.ValueChanged += OnValueChanged;
        }

        public void AbsorbContent(List<IContent> contentsIter)
        {
            // This check prevents a repeater from getting added twice.
            // This can happen in the following situation:
            //    RECTANGLE
            //    REPEATER 1
            //    FILL
            //    REPEATER 2
            // In this case, the expected structure would be:
            //     REPEATER 2
            //        REPEATER 1
            //            RECTANGLE
            //        FILL
            // Without this check, REPEATER 1 will try and absorb contents once it is already inside of
            // REPEATER 2.
            if (_contentGroup != null)
            {
                return;
            }

            // Fast forward the iterator until after this content.
            var index = contentsIter.Count;
            while (index > 0)
            {
                index--;
                if (contentsIter[index] == this)
                {
                    break;
                }
            }

            var contents = new List<IContent>();
            while (index > 0)
            {
                index--;
                contents.Add(contentsIter[index]);
                contentsIter.RemoveAt(index);
            }

            contents.Reverse();
            _contentGroup = new ContentGroup(_lottieDrawable, _layer, "Repeater", contents, null);
        }

        public string Name { get; }

        public void SetContents(List<IContent> contentsBefore, List<IContent> contentsAfter)
        {
            _contentGroup.SetContents(contentsBefore, contentsAfter);
        }

        public Path Path
        {
            get
            {
                var contentPath = _contentGroup.Path;
                _path.Reset();
                var copies = _copies.Value.Value;
                var offset = _offset.Value.Value;
                for (var i = (int)copies - 1; i >= 0; i--)
                {
                    _matrix.Set(_transform.GetMatrixForRepeater(i + offset));
                    _path.AddPath(contentPath, _matrix);
                }

                return _path;
            }
        }

        public void Draw(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte alpha)
        {
            var copies = _copies.Value.Value;
            var offset = _offset.Value.Value;
            var startOpacity = _transform.StartOpacity.Value.Value / 100f;
            var endOpacity = _transform.EndOpacity.Value.Value / 100f;
            for (var i = (int)copies - 1; i >= 0; i--)
            {
                _matrix.Set(parentMatrix);
                _matrix = MatrixExt.PreConcat(_matrix, _transform.GetMatrixForRepeater(i + offset));
                var newAlpha = alpha * MiscUtils.Lerp(startOpacity, endOpacity, i / copies);
                _contentGroup.Draw(canvas, _matrix, (byte)newAlpha);
            }
        }

        public void GetBounds(out Rect outBounds, Matrix3X3 parentMatrix)
        {
            _contentGroup.GetBounds(out outBounds, parentMatrix);
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            _lottieDrawable.InvalidateSelf();
        }

        public void ResolveKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath)
        {
            MiscUtils.ResolveKeyPath(keyPath, depth, accumulator, currentPartialKeyPath, this);
        }

        public void AddValueCallback<T>(LottieProperty property, ILottieValueCallback<T> callback)
        {
            if (_transform.ApplyValueCallback(property, callback))
            {
                return;
            }

            if (property == LottieProperty.RepeaterCopies)
            {
                _copies.SetValueCallback((ILottieValueCallback<float?>)callback);
            }
            else if (property == LottieProperty.RepeaterOffset)
            {
                _offset.SetValueCallback((ILottieValueCallback<float?>)callback);
            }
        }
    }
}
