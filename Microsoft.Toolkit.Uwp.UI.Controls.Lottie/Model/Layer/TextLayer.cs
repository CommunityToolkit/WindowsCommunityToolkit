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

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.Foundation;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer
{
    internal class TextLayer : BaseLayer
    {
        // private Rect _rectF;
        private readonly Paint _fillPaint = new Paint(Paint.AntiAliasFlag)
        {
            Style = Paint.PaintStyle.Fill
        };

        private readonly Paint _strokePaint = new Paint(Paint.AntiAliasFlag)
        {
            Style = Paint.PaintStyle.Stroke
        };

        private readonly Dictionary<FontCharacter, List<ContentGroup>> _contentsForCharacter = new Dictionary<FontCharacter, List<ContentGroup>>();
        private readonly TextKeyframeAnimation _textAnimation;
        private readonly LottieDrawable _lottieDrawable;
        private readonly LottieComposition _composition;
        private readonly IBaseKeyframeAnimation<Color?, Color?> _colorAnimation;
        private readonly IBaseKeyframeAnimation<Color?, Color?> _strokeColorAnimation;
        private readonly IBaseKeyframeAnimation<float?, float?> _strokeWidthAnimation;
        private readonly IBaseKeyframeAnimation<float?, float?> _trackingAnimation;

        internal TextLayer(LottieDrawable lottieDrawable, Layer layerModel)
            : base(lottieDrawable, layerModel)
        {
            _lottieDrawable = lottieDrawable;
            _composition = layerModel.Composition;
            _textAnimation = (TextKeyframeAnimation)layerModel.Text.CreateAnimation();
            _textAnimation.ValueChanged += OnValueChanged;
            AddAnimation(_textAnimation);

            var textProperties = layerModel.TextProperties;
            if (textProperties?.Color != null)
            {
                _colorAnimation = textProperties.Color.CreateAnimation();
                _colorAnimation.ValueChanged += OnValueChanged;
                AddAnimation(_colorAnimation);
            }

            if (textProperties?.Stroke != null)
            {
                _strokeColorAnimation = textProperties.Stroke.CreateAnimation();
                _strokeColorAnimation.ValueChanged += OnValueChanged;
                AddAnimation(_strokeColorAnimation);
            }

            if (textProperties?.StrokeWidth != null)
            {
                _strokeWidthAnimation = textProperties.StrokeWidth.CreateAnimation();
                _strokeWidthAnimation.ValueChanged += OnValueChanged;
                AddAnimation(_strokeWidthAnimation);
            }

            if (textProperties?.Tracking != null)
            {
                _trackingAnimation = textProperties.Tracking.CreateAnimation();
                _trackingAnimation.ValueChanged += OnValueChanged;
                AddAnimation(_trackingAnimation);
            }
        }

        public override void DrawLayer(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte parentAlpha)
        {
            canvas.Save();
            if (!_lottieDrawable.UseTextGlyphs())
            {
                canvas.SetMatrix(parentMatrix);
            }

            var documentData = _textAnimation.Value;
            if (!_composition.Fonts.TryGetValue(documentData.FontName, out var font))
            {
                // Something is wrong.
                canvas.Restore();
                return;
            }

            _fillPaint.Color = _colorAnimation?.Value ?? documentData.Color;

            _strokePaint.Color = _strokeColorAnimation?.Value ?? documentData.StrokeColor;
            var alpha = (byte)(Transform.Opacity.Value * 255 / 100f);
            _fillPaint.Alpha = alpha;
            _strokePaint.Alpha = alpha;

            if (_strokeWidthAnimation?.Value != null)
            {
                _strokePaint.StrokeWidth = _strokeWidthAnimation.Value.Value;
            }
            else
            {
                var parentScale = Utils.Utils.GetScale(parentMatrix);
                _strokePaint.StrokeWidth = documentData.StrokeWidth * Utils.Utils.DpScale() * parentScale;
            }

            if (_lottieDrawable.UseTextGlyphs())
            {
                DrawTextGlyphs(documentData, parentMatrix, font, canvas);
            }
            else
            {
                DrawTextWithFont(documentData, font, parentMatrix, canvas);
            }

            canvas.Restore();
        }

        private void DrawTextGlyphs(DocumentData documentData, Matrix3X3 parentMatrix, Font font, BitmapCanvas canvas)
        {
            var fontScale = (float)documentData.Size / 100;
            var parentScale = Utils.Utils.GetScale(parentMatrix);
            var text = documentData.Text;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var characterHash = FontCharacter.HashFor(c, font.Family, font.Style);
                if (!_composition.Characters.TryGetValue(characterHash, out var character))
                {
                    // Something is wrong. Potentially, they didn't export the text as a glyph.
                    continue;
                }

                DrawCharacterAsGlyph(character, parentMatrix, fontScale, documentData, canvas);
                var tx = (float)character.Width * fontScale * Utils.Utils.DpScale() * parentScale;

                // Add tracking
                var tracking = documentData.Tracking / 10f;
                if (_trackingAnimation?.Value != null)
                {
                    tracking += _trackingAnimation.Value.Value;
                }

                tx += tracking * parentScale;
                canvas.Translate(tx, 0);
            }
        }

        private void DrawTextWithFont(DocumentData documentData, Font font, Matrix3X3 parentMatrix, BitmapCanvas canvas)
        {
            var parentScale = Utils.Utils.GetScale(parentMatrix);
            var typeface = _lottieDrawable.GetTypeface(font.Family, font.Style);
            if (typeface == null)
            {
                return;
            }

            var text = documentData.Text;
            var textDelegate = _lottieDrawable.TextDelegate;
            if (textDelegate != null)
            {
                text = textDelegate.GetTextInternal(text);
            }

            _fillPaint.Typeface = typeface;
            _fillPaint.TextSize = (float)documentData.Size * Utils.Utils.DpScale();
            _strokePaint.Typeface = _fillPaint.Typeface;
            _strokePaint.TextSize = _fillPaint.TextSize;
            for (var i = 0; i < text.Length; i++)
            {
                var character = text[i];
                var size = DrawCharacterFromFont(character, documentData, canvas);

                // Add tracking
                var tracking = documentData.Tracking / 10f;
                if (_trackingAnimation?.Value != null)
                {
                    tracking += _trackingAnimation.Value.Value;
                }

                var tx = (float)(size?.Width ?? 0) + (tracking * parentScale);
                canvas.Translate(tx, 0);
            }
        }

        private void DrawCharacterAsGlyph(FontCharacter character, Matrix3X3 parentMatrix, float fontScale, DocumentData documentData, BitmapCanvas canvas)
        {
            var contentGroups = GetContentsForCharacter(character);
            for (var j = 0; j < contentGroups.Count; j++)
            {
                var path = contentGroups[j].Path;

                // path.ComputeBounds(out _rectF);
                Matrix.Set(parentMatrix);
                Matrix = MatrixExt.PreTranslate(Matrix, 0, (float)-documentData.BaselineShift * Utils.Utils.DpScale());
                Matrix = MatrixExt.PreScale(Matrix, fontScale, fontScale);
                path.Transform(Matrix);
                if (documentData.StrokeOverFill)
                {
                    DrawGlyph(path, _fillPaint, canvas);
                    DrawGlyph(path, _strokePaint, canvas);
                }
                else
                {
                    DrawGlyph(path, _strokePaint, canvas);
                    DrawGlyph(path, _fillPaint, canvas);
                }
            }
        }

        private void DrawGlyph(Path path, Paint paint, BitmapCanvas canvas)
        {
            if (paint.Color == Colors.Transparent)
            {
                return;
            }

            if (paint.Style == Paint.PaintStyle.Stroke && paint.StrokeWidth == 0)
            {
                return;
            }

            canvas.DrawPath(path, paint);
        }

        private Rect? DrawCharacterFromFont(char c, DocumentData documentData, BitmapCanvas canvas)
        {
            Rect? ret;
            if (documentData.StrokeOverFill)
            {
                ret = DrawCharacter(c, _fillPaint, canvas);
                return DrawCharacter(c, _strokePaint, canvas) ?? ret;
            }

            ret = DrawCharacter(c, _strokePaint, canvas);
            return DrawCharacter(c, _fillPaint, canvas) ?? ret;
        }

        private Rect? DrawCharacter(char character, Paint paint, BitmapCanvas canvas)
        {
            if (paint.Color == Colors.Transparent)
            {
                return null;
            }

            if (paint.Style == Paint.PaintStyle.Stroke && paint.StrokeWidth == 0)
            {
                return null;
            }

            return canvas.DrawText(character, paint);
        }

        private List<ContentGroup> GetContentsForCharacter(FontCharacter character)
        {
            if (_contentsForCharacter.ContainsKey(character))
            {
                return _contentsForCharacter[character];
            }

            var shapes = character.Shapes;
            var size = shapes.Count;
            var contents = new List<ContentGroup>(size);
            for (var i = 0; i < size; i++)
            {
                var sg = shapes[i];
                contents.Add(new ContentGroup(_lottieDrawable, this, sg));
            }

            _contentsForCharacter[character] = contents;
            return contents;
        }

        public override void AddValueCallback<T>(LottieProperty property, ILottieValueCallback<T> callback)
        {
            base.AddValueCallback(property, callback);
            if (property == LottieProperty.Color && _colorAnimation != null)
            {
                _colorAnimation.SetValueCallback((ILottieValueCallback<Color?>)callback);
            }
            else if (property == LottieProperty.StrokeColor && _strokeColorAnimation != null)
            {
                _strokeColorAnimation.SetValueCallback((ILottieValueCallback<Color?>)callback);
            }
            else if (property == LottieProperty.StrokeWidth && _strokeWidthAnimation != null)
            {
                _strokeWidthAnimation.SetValueCallback((ILottieValueCallback<float?>)callback);
            }
            else if (property == LottieProperty.TextTracking)
            {
                _trackingAnimation?.SetValueCallback((ILottieValueCallback<float?>)callback);
            }
        }
    }
}
