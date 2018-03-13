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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Keyframe;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer
{
    internal abstract class BaseLayer : IDrawingContent, IKeyPathElement
    {
        private static readonly int SaveFlags = BitmapCanvas.ClipSaveFlag | BitmapCanvas.ClipToLayerSaveFlag | BitmapCanvas.MatrixSaveFlag;

        internal static BaseLayer ForModel(Layer layerModel, LottieDrawable drawable, LottieComposition composition)
        {
            switch (layerModel.GetLayerType())
            {
                case Layer.LayerType.Shape:
                    return new ShapeLayer(drawable, layerModel);
                case Layer.LayerType.PreComp:
                    return new CompositionLayer(drawable, layerModel, composition.GetPrecomps(layerModel.RefId), composition);
                case Layer.LayerType.Solid:
                    return new SolidLayer(drawable, layerModel);
                case Layer.LayerType.Image:
                    return new ImageLayer(drawable, layerModel);
                case Layer.LayerType.Null:
                    return new NullLayer(drawable, layerModel);
                case Layer.LayerType.Text:
                    return new TextLayer(drawable, layerModel);
                case Layer.LayerType.Unknown:
                default:
                    // Do nothing
                    LottieLog.Warn("Unknown layer type " + layerModel.GetLayerType());
                    return null;
            }
        }

        private readonly Path _path = new Path();
        private readonly Paint _contentPaint = new Paint(Paint.AntiAliasFlag);
        private readonly Paint _addMaskPaint = new Paint(Paint.AntiAliasFlag);
        private readonly Paint _subtractMaskPaint = new Paint(Paint.AntiAliasFlag);
        private readonly Paint _mattePaint = new Paint(Paint.AntiAliasFlag);
        private readonly Paint _clearPaint = new Paint();
        private readonly string _drawTraceName;
        private readonly MaskKeyframeAnimation _mask;
        private readonly List<IBaseKeyframeAnimation> _animations = new List<IBaseKeyframeAnimation>();
        private readonly LottieDrawable _lottieDrawable;
        private readonly TransformKeyframeAnimation _transform;

        internal TransformKeyframeAnimation Transform => _transform;

        internal Matrix3X3 Matrix { get; set; } = Matrix3X3.CreateIdentity();

        internal Matrix3X3 BoundsMatrix { get; set; } = Matrix3X3.CreateIdentity();

        internal Layer LayerModel { get; }

#pragma warning disable SA1401 // Fields must be private
        internal Rect Rect;
#pragma warning restore SA1401 // Fields must be private

        private Rect _maskBoundsRect;
        private Rect _matteBoundsRect;
        private Rect _tempMaskBoundsRect;
        private BaseLayer _matteLayer;
        private BaseLayer _parentLayer;
        private List<BaseLayer> _parentLayers;
        private bool _visible = true;

        internal BaseLayer(LottieDrawable lottieDrawable, Layer layerModel)
        {
            _lottieDrawable = lottieDrawable;
            LayerModel = layerModel;
            _drawTraceName = layerModel.Name + ".Draw";
            _contentPaint.Alpha = 255;
            _clearPaint.Xfermode = new PorterDuffXfermode(PorterDuff.Mode.Clear);
            _addMaskPaint.Xfermode = new PorterDuffXfermode(PorterDuff.Mode.DstIn);
            _subtractMaskPaint.Xfermode = new PorterDuffXfermode(PorterDuff.Mode.DstOut);
            if (layerModel.GetMatteType() == Layer.MatteType.Invert)
            {
                _mattePaint.Xfermode = new PorterDuffXfermode(PorterDuff.Mode.DstOut);
            }
            else
            {
                _mattePaint.Xfermode = new PorterDuffXfermode(PorterDuff.Mode.DstIn);
            }

            _transform = layerModel.Transform.CreateAnimation();
            Transform.ValueChanged += OnValueChanged;

            if (layerModel.Masks != null && layerModel.Masks.Count > 0)
            {
                _mask = new MaskKeyframeAnimation(layerModel.Masks);
                foreach (var animation in _mask.MaskAnimations)
                {
                    AddAnimation(animation);
                    animation.ValueChanged += OnValueChanged;
                }

                foreach (var animation in _mask.OpacityAnimations)
                {
                    AddAnimation(animation);
                    animation.ValueChanged += OnValueChanged;
                }
            }

            SetupInOutAnimations();
        }

        public virtual void OnValueChanged(object sender, EventArgs eventArgs)
        {
            InvalidateSelf();
        }

        internal virtual BaseLayer MatteLayer
        {
            set => _matteLayer = value;
        }

        internal virtual bool HasMatteOnThisLayer()
        {
            return _matteLayer != null;
        }

        internal virtual BaseLayer ParentLayer
        {
            set => _parentLayer = value;
        }

        private void SetupInOutAnimations()
        {
            if (LayerModel.InOutKeyframes.Count > 0)
            {
                var inOutAnimation = new FloatKeyframeAnimation(LayerModel.InOutKeyframes);
                inOutAnimation.SetIsDiscrete();
                inOutAnimation.ValueChanged += (sender, args) =>
                {
                    Visible = inOutAnimation.Value == 1f;
                };
                Visible = inOutAnimation.Value == 1f;
                AddAnimation(inOutAnimation);
            }
            else
            {
                Visible = true;
            }
        }

        private void InvalidateSelf()
        {
            LottieDrawable.InvalidateSelf();
        }

        internal void AddAnimation(IBaseKeyframeAnimation newAnimation)
        {
            _animations.Add(newAnimation);
        }

        public virtual void GetBounds(out Rect outBounds, Matrix3X3 parentMatrix)
        {
            BoundsMatrix.Set(parentMatrix);
            BoundsMatrix = MatrixExt.PreConcat(BoundsMatrix, Transform.Matrix);
        }

        public void Draw(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte parentAlpha)
        {
            LottieLog.BeginSection(_drawTraceName);
            if (!_visible)
            {
                LottieLog.EndSection(_drawTraceName);
                return;
            }

            BuildParentLayerListIfNeeded();
            LottieLog.BeginSection("Layer.ParentMatrix");
            Matrix.Reset();
            Matrix.Set(parentMatrix);
            for (var i = _parentLayers.Count - 1; i >= 0; i--)
            {
                Matrix = MatrixExt.PreConcat(Matrix, _parentLayers[i].Transform.Matrix);
            }

            LottieLog.EndSection("Layer.ParentMatrix");
            var alpha = (byte)(parentAlpha / 255f * (float)Transform.Opacity.Value / 100f * 255);
            if (!HasMatteOnThisLayer() && !HasMasksOnThisLayer())
            {
                Matrix = MatrixExt.PreConcat(Matrix, Transform.Matrix);
                LottieLog.BeginSection("Layer.DrawLayer");
                DrawLayer(canvas, Matrix, alpha);
                LottieLog.EndSection("Layer.DrawLayer");
                RecordRenderTime(LottieLog.EndSection(_drawTraceName));
                return;
            }

            LottieLog.BeginSection("Layer.ComputeBounds");
            RectExt.Set(ref Rect, 0, 0, 0, 0);
            GetBounds(out Rect, Matrix);
            IntersectBoundsWithMatte(ref Rect, Matrix);

            Matrix = MatrixExt.PreConcat(Matrix, Transform.Matrix);
            IntersectBoundsWithMask(ref Rect, Matrix);

            RectExt.Set(ref Rect, 0, 0, canvas.Width, canvas.Height);
            LottieLog.EndSection("Layer.ComputeBounds");

            LottieLog.BeginSection("Layer.SaveLayer");
            canvas.SaveLayer(Rect, _contentPaint, BitmapCanvas.AllSaveFlag);
            LottieLog.EndSection("Layer.SaveLayer");

            // Clear the off screen buffer. This is necessary for some phones.
            ClearCanvas(canvas);
            LottieLog.BeginSection("Layer.DrawLayer");
            DrawLayer(canvas, Matrix, alpha);
            LottieLog.EndSection("Layer.DrawLayer");

            if (HasMasksOnThisLayer())
            {
                ApplyMasks(canvas, Matrix);
            }

            if (HasMatteOnThisLayer())
            {
                LottieLog.BeginSection("Layer.DrawMatte");
                LottieLog.BeginSection("Layer.SaveLayer");
                canvas.SaveLayer(Rect, _mattePaint, SaveFlags);
                LottieLog.EndSection("Layer.SaveLayer");
                ClearCanvas(canvas);

                _matteLayer.Draw(canvas, parentMatrix, alpha);
                LottieLog.BeginSection("Layer.RestoreLayer");
                canvas.Restore();
                LottieLog.EndSection("Layer.RestoreLayer");
                LottieLog.EndSection("Layer.DrawMatte");
            }

            LottieLog.BeginSection("Layer.RestoreLayer");
            canvas.Restore();
            LottieLog.EndSection("Layer.RestoreLayer");
            RecordRenderTime(LottieLog.EndSection(_drawTraceName));
        }

        private void RecordRenderTime(float ms)
        {
            LottieDrawable.Composition.PerformanceTracker.RecordRenderTime(LayerModel.Name, ms);
        }

        private void ClearCanvas(BitmapCanvas canvas)
        {
            LottieLog.BeginSection("Layer.ClearLayer");

            // If we don't pad the clear draw, some phones leave a 1px border of the graphics buffer.
            canvas.DrawRect(Rect.Left - 1, Rect.Top - 1, Rect.Right + 1, Rect.Bottom + 1, _clearPaint);
            LottieLog.EndSection("Layer.ClearLayer");
        }

        private void IntersectBoundsWithMask(ref Rect rect, Matrix3X3 matrix)
        {
            RectExt.Set(ref _maskBoundsRect, 0, 0, 0, 0);
            if (!HasMasksOnThisLayer())
            {
                return;
            }

            var size = _mask.Masks.Count;
            for (var i = 0; i < size; i++)
            {
                var mask = _mask.Masks[i];
                var maskAnimation = _mask.MaskAnimations[i];
                var maskPath = maskAnimation.Value;
                _path.Set(maskPath);
                _path.Transform(matrix);

                switch (mask.GetMaskMode())
                {
                    case Mask.MaskMode.MaskModeSubtract:
                        // If there is a subtract mask, the mask could potentially be the size of the entire
                        // canvas so we can't use the mask bounds.
                        return;
                    case Mask.MaskMode.MaskModeIntersect:
                        // TODO
                        return;
                    case Mask.MaskMode.MaskModeAdd:
                    default:
                        _path.ComputeBounds(out _tempMaskBoundsRect);

                        // As we iterate through the masks, we want to calculate the union region of the masks.
                        // We initialize the rect with the first mask. If we don't call set() on the first call,
                        // the rect will always extend to (0,0).
                        if (i == 0)
                        {
                            RectExt.Set(ref _maskBoundsRect, _tempMaskBoundsRect);
                        }
                        else
                        {
                            RectExt.Set(ref _maskBoundsRect, Math.Min(_maskBoundsRect.Left, _tempMaskBoundsRect.Left), Math.Min(_maskBoundsRect.Top, _tempMaskBoundsRect.Top), Math.Max(_maskBoundsRect.Right, _tempMaskBoundsRect.Right), Math.Max(_maskBoundsRect.Bottom, _tempMaskBoundsRect.Bottom));
                        }

                        break;
                }
            }

            RectExt.Set(ref rect, Math.Max(rect.Left, _maskBoundsRect.Left), Math.Max(rect.Top, _maskBoundsRect.Top), Math.Min(rect.Right, _maskBoundsRect.Right), Math.Min(rect.Bottom, _maskBoundsRect.Bottom));
        }

        private void IntersectBoundsWithMatte(ref Rect rect, Matrix3X3 matrix)
        {
            if (!HasMatteOnThisLayer())
            {
                return;
            }

            if (LayerModel.GetMatteType() == Layer.MatteType.Invert)
            {
                // We can't trim the bounds if the mask is inverted since it extends all the way to the
                // composition bounds.
                return;
            }

            _matteLayer.GetBounds(out _matteBoundsRect, matrix);
            RectExt.Set(ref rect, Math.Max(rect.Left, _matteBoundsRect.Left), Math.Max(rect.Top, _matteBoundsRect.Top), Math.Min(rect.Right, _matteBoundsRect.Right), Math.Min(rect.Bottom, _matteBoundsRect.Bottom));
        }

        public abstract void DrawLayer(BitmapCanvas canvas, Matrix3X3 parentMatrix, byte parentAlpha);

        private void ApplyMasks(BitmapCanvas canvas, Matrix3X3 matrix)
        {
            ApplyMasks(canvas, matrix, Mask.MaskMode.MaskModeAdd);

            // Treat intersect masks like add masks. This is not correct but it's closer.
            ApplyMasks(canvas, matrix, Mask.MaskMode.MaskModeIntersect);
            ApplyMasks(canvas, matrix, Mask.MaskMode.MaskModeSubtract);
        }

        private void ApplyMasks(BitmapCanvas canvas, Matrix3X3 matrix, Mask.MaskMode maskMode)
        {
            Paint paint;
            switch (maskMode)
            {
                case Mask.MaskMode.MaskModeSubtract:
                    paint = _subtractMaskPaint;
                    break;
                case Mask.MaskMode.MaskModeIntersect:
                    Debug.WriteLine(
                        "Animation contains intersect masks. They are not supported but will be " +
                                 "treated like add masks.", LottieLog.Tag);
                    goto case Mask.MaskMode.MaskModeAdd;
                case Mask.MaskMode.MaskModeAdd:
                default:
                    // As a hack, we treat all non-subtract masks like add masks. This is not correct but it's
                    // better than nothing.
                    paint = _addMaskPaint;
                    break;
            }

            var size = _mask.Masks.Count;

            var hasMask = false;
            for (int i = 0; i < size; i++)
            {
                if (_mask.Masks[i].GetMaskMode() == maskMode)
                {
                    hasMask = true;
                    break;
                }
            }

            if (!hasMask)
            {
                return;
            }

            LottieLog.BeginSection("Layer.DrawMask");
            LottieLog.BeginSection("Layer.SaveLayer");
            canvas.SaveLayer(Rect, paint, SaveFlags);
            LottieLog.EndSection("Layer.SaveLayer");
            ClearCanvas(canvas);

            for (var i = 0; i < size; i++)
            {
                var mask = _mask.Masks[i];
                if (mask.GetMaskMode() != maskMode)
                {
                    continue;
                }

                var maskAnimation = _mask.MaskAnimations[i];
                var maskPath = maskAnimation.Value;
                _path.Set(maskPath);
                _path.Transform(matrix);
                var opacityAnimation = _mask.OpacityAnimations[i];
                var alpha = _contentPaint.Alpha;
                _contentPaint.Alpha = (byte)(opacityAnimation.Value.Value * 2.55f);
                canvas.DrawPath(_path, _contentPaint);
                _contentPaint.Alpha = alpha;
            }

            LottieLog.BeginSection("Layer.RestoreLayer");
            canvas.Restore();
            LottieLog.EndSection("Layer.RestoreLayer");
            LottieLog.EndSection("Layer.DrawMask");
        }

        internal virtual bool HasMasksOnThisLayer()
        {
            return _mask != null && _mask.MaskAnimations.Count > 0;
        }

        private bool Visible
        {
            set
            {
                if (value != _visible)
                {
                    _visible = value;
                    InvalidateSelf();
                }
            }
        }

        public virtual float Progress
        {
            set
            {
                // Time stretch should not be applied to the layer transform.
                Transform.Progress = value;
                if (LayerModel.TimeStretch != 0)
                {
                    value /= LayerModel.TimeStretch;
                }

                if (_matteLayer != null)
                {
                    // The matte layer's time stretch is pre-calculated.
                    float matteTimeStretch = _matteLayer.LayerModel.TimeStretch;
                    _matteLayer.Progress = value * matteTimeStretch;
                }

                for (var i = 0; i < _animations.Count; i++)
                {
                    _animations[i].Progress = value;
                }
            }
        }

        private void BuildParentLayerListIfNeeded()
        {
            if (_parentLayers != null)
            {
                return;
            }

            if (_parentLayer == null)
            {
                _parentLayers = new List<BaseLayer>();
                return;
            }

            _parentLayers = new List<BaseLayer>();
            var layer = _parentLayer;
            while (layer != null)
            {
                _parentLayers.Add(layer);
                layer = layer._parentLayer;
            }
        }

        public string Name => LayerModel.Name;

        internal LottieDrawable LottieDrawable => _lottieDrawable;

        public void SetContents(List<IContent> contentsBefore, List<IContent> contentsAfter)
        {
            // Do nothing
        }

        public void ResolveKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath)
        {
            if (!keyPath.Matches(Name, depth))
            {
                return;
            }

            if (!"__container".Equals(Name))
            {
                currentPartialKeyPath = currentPartialKeyPath.AddKey(Name);

                if (keyPath.FullyResolvesTo(Name, depth))
                {
                    accumulator.Add(currentPartialKeyPath.Resolve(this));
                }
            }

            if (keyPath.PropagateToChildren(Name, depth))
            {
                int newDepth = depth + keyPath.IncrementDepthBy(Name, depth);
                ResolveChildKeyPath(keyPath, newDepth, accumulator, currentPartialKeyPath);
            }
        }

        internal virtual void ResolveChildKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath)
        {
        }

        public virtual void AddValueCallback<T>(LottieProperty property, ILottieValueCallback<T> callback)
        {
            Transform.ApplyValueCallback(property, callback);
        }
    }
}