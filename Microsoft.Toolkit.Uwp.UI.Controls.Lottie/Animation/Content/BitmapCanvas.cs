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
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class BitmapCanvas : IDisposable
    {
        private readonly Stack<Matrix3X3> _matrixSaves = new Stack<Matrix3X3>();
        private readonly Stack<int> _flagSaves = new Stack<int>();
        private readonly Dictionary<int, CanvasRenderTarget> _renderTargets = new Dictionary<int, CanvasRenderTarget>();

        private Matrix3X3 _matrix = Matrix3X3.CreateIdentity();

        private class ClipSave
        {
            public ClipSave(Rect rect, CanvasActiveLayer layer)
            {
                Rect = rect;
                Layer = layer;
            }

            public Rect Rect { get; }

            public CanvasActiveLayer Layer { get; }
        }

        private readonly Stack<ClipSave> _clipSaves = new Stack<ClipSave>();
        private Rect _currentClip;

        private class RenderTargetSave
        {
            public RenderTargetSave(CanvasRenderTarget canvasRenderTarget, int paintFlags, PorterDuffXfermode paintXfermode, byte paintAlpha)
            {
                CanvasRenderTarget = canvasRenderTarget;
                PaintFlags = paintFlags;
                PaintXfermode = paintXfermode;
                PaintAlpha = paintAlpha;
            }

            public CanvasRenderTarget CanvasRenderTarget { get; }

            public int PaintFlags { get; }

            public PorterDuffXfermode PaintXfermode { get; }

            public byte PaintAlpha { get; }
        }

        private readonly Stack<RenderTargetSave> _renderTargetSaves = new Stack<RenderTargetSave>();
        private readonly Stack<CanvasDrawingSession> _canvasDrawingSessions = new Stack<CanvasDrawingSession>();

        internal BitmapCanvas(double width, double height)
        {
            UpdateClip(width, height);
        }

        private void UpdateClip(double width, double height)
        {
            if (Math.Abs(width - Width) > 0 || Math.Abs(height - Height) > 0)
            {
                Dispose(false);
            }

            Width = width;
            Height = height;
            _currentClip = new Rect(0, 0, Width, Height);
        }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public const int MatrixSaveFlag = 0b00001;
        public const int ClipSaveFlag = 0b00010;

        // public const int HasAlphaLayerSaveFlag = 0b00100;
        // public const int FullColorLayerSaveFlag = 0b01000;
        public const int ClipToLayerSaveFlag = 0b10000;
        public const int AllSaveFlag = 0b11111;

        private CanvasDevice _device;

        private CanvasDrawingSession CurrentDrawingSession => _canvasDrawingSessions.Peek();

        internal CanvasActiveLayer CreateSession(CanvasDevice device, double width, double height, CanvasDrawingSession drawingSession)
        {
            _device = device;
            _canvasDrawingSessions.Clear();
            _canvasDrawingSessions.Push(drawingSession);

            UpdateClip(width, height);

            return CurrentDrawingSession.CreateLayer(1f, _currentClip);
        }

        public void DrawRect(double x1, double y1, double x2, double y2, Paint paint)
        {
            UpdateDrawingSessionWithFlags(paint.Flags);
            CurrentDrawingSession.Transform = GetCurrentTransform();
            using (var brush = new CanvasSolidColorBrush(_device, paint.Color))
            {
                if (paint.Style == Paint.PaintStyle.Stroke)
                {
                    CurrentDrawingSession.DrawRectangle((float)x1, (float)y1, (float)(x2 - x1), (float)(y2 - y1), brush, paint.StrokeWidth, GetCanvasStrokeStyle(paint));
                }
                else
                {
                    CurrentDrawingSession.FillRectangle((float)x1, (float)y1, (float)(x2 - x1), (float)(y2 - y1), brush);
                }
            }

            if (paint.Xfermode.Mode == PorterDuff.Mode.Clear)
            {
                CurrentDrawingSession.Flush();
            }
        }

        private static CanvasStrokeStyle GetCanvasStrokeStyle(Paint paint)
        {
            var style = new CanvasStrokeStyle
            {
                StartCap = paint.StrokeCap,
                DashCap = paint.StrokeCap,
                EndCap = paint.StrokeCap,
                LineJoin = paint.StrokeJoin,
            };
            paint.PathEffect?.Apply(style, paint);
            return style;
        }

        internal void DrawRect(Rect rect, Paint paint)
        {
            UpdateDrawingSessionWithFlags(paint.Flags);
            CurrentDrawingSession.Transform = GetCurrentTransform();

            using (var brush = new CanvasSolidColorBrush(_device, paint.Color))
            {
                if (paint.Style == Paint.PaintStyle.Stroke)
                {
                    CurrentDrawingSession.DrawRectangle(rect, brush, paint.StrokeWidth, GetCanvasStrokeStyle(paint));
                }
                else
                {
                    CurrentDrawingSession.FillRectangle(rect, brush);
                }
            }
        }

        public void DrawPath(Path path, Paint paint)
        {
            UpdateDrawingSessionWithFlags(paint.Flags);

            CurrentDrawingSession.Transform = GetCurrentTransform();

            var gradient = paint.Shader as Gradient;
            var brush = gradient != null ? gradient.GetBrush(_device, paint.Alpha) : new CanvasSolidColorBrush(_device, paint.Color);
            brush = paint.ColorFilter?.Apply(_device, brush) ?? brush;

            using (var geometry = path.GetGeometry(_device))
            {
                if (paint.Style == Paint.PaintStyle.Stroke)
                {
                    CurrentDrawingSession.DrawGeometry(geometry, brush, paint.StrokeWidth, GetCanvasStrokeStyle(paint));
                }
                else
                {
                    CurrentDrawingSession.FillGeometry(geometry, brush);
                }
            }

            if (gradient == null || paint.ColorFilter != null)
            {
                brush.Dispose();
            }
        }

        private Matrix3x2 GetCurrentTransform()
        {
            return new Matrix3x2
            {
                M11 = _matrix.M11,
                M12 = _matrix.M21,
                M21 = _matrix.M12,
                M22 = _matrix.M22,
                M31 = _matrix.M13,
                M32 = _matrix.M23
            };
        }

        public bool ClipRect(Rect rect)
        {
            _currentClip.Intersect(rect);
            return _currentClip.IsEmpty == false;
        }

        public void ClipReplaceRect(Rect rect)
        {
            _currentClip = rect;
        }

        public void Concat(Matrix3X3 parentMatrix)
        {
            _matrix = MatrixExt.PreConcat(_matrix, parentMatrix);
        }

        public void Save()
        {
            _flagSaves.Push(MatrixSaveFlag | ClipSaveFlag);
            SaveMatrix();
            SaveClip(255);
        }

        public void SaveLayer(Rect bounds, Paint paint, int flags)
        {
            _flagSaves.Push(flags);
            if ((flags & MatrixSaveFlag) == MatrixSaveFlag)
            {
                SaveMatrix();
            }

            var isClipToLayer = (flags & ClipToLayerSaveFlag) == ClipToLayerSaveFlag;
            if ((flags & ClipSaveFlag) == ClipSaveFlag)
            {
                SaveClip(isClipToLayer ? (byte)255 : paint.Alpha);
            }

            if (isClipToLayer)
            {
                UpdateDrawingSessionWithFlags(paint.Flags);

                var rendertarget = CreateCanvasRenderTarget(bounds, _renderTargetSaves.Count);
                _renderTargetSaves.Push(new RenderTargetSave(rendertarget, paint.Flags, paint.Xfermode, paint.Xfermode != null ? (byte)255 : paint.Alpha));

                var drawingSession = rendertarget.CreateDrawingSession();
                drawingSession.Clear(Colors.Transparent);
                _canvasDrawingSessions.Push(drawingSession);
            }
        }

        private CanvasRenderTarget CreateCanvasRenderTarget(Rect bounds, int index)
        {
            if (!_renderTargets.TryGetValue(index, out var rendertarget))
            {
                rendertarget = new CanvasRenderTarget(_device, (float)bounds.Width, (float)bounds.Height, Utils.Utils.Dpi(), DirectXPixelFormat.B8G8R8A8UIntNormalized, CanvasAlphaMode.Premultiplied);
                _renderTargets.Add(index, rendertarget);
            }

            return rendertarget;
        }

        private void SaveMatrix()
        {
            var copy = new Matrix3X3();
            copy.Set(_matrix);
            _matrixSaves.Push(copy);
        }

        private void SaveClip(byte alpha)
        {
            var currentLayer = CurrentDrawingSession.CreateLayer(alpha / 255f, _currentClip);

            _clipSaves.Push(new ClipSave(_currentClip, currentLayer));
        }

        public void Restore()
        {
            var flags = _flagSaves.Pop();
            if ((flags & MatrixSaveFlag) == MatrixSaveFlag)
            {
                _matrix = _matrixSaves.Pop();
            }

            if ((flags & ClipSaveFlag) == ClipSaveFlag)
            {
                var clipSave = _clipSaves.Pop();
                _currentClip = clipSave.Rect;
                clipSave.Layer.Dispose();
            }

            if ((flags & ClipToLayerSaveFlag) == ClipToLayerSaveFlag)
            {
                var drawingSession = _canvasDrawingSessions.Pop();
                drawingSession.Flush();
                drawingSession.Dispose();

                var renderTargetSave = _renderTargetSaves.Pop();

                var rt = renderTargetSave.CanvasRenderTarget;

                UpdateDrawingSessionWithFlags(renderTargetSave.PaintFlags);
                CurrentDrawingSession.Transform = GetCurrentTransform();

                var canvasComposite = CanvasComposite.SourceAtop;
                if (renderTargetSave.PaintXfermode != null)
                {
                    canvasComposite = PorterDuff.ToCanvasComposite(renderTargetSave.PaintXfermode.Mode);
                }

                CurrentDrawingSession.DrawImage(rt, 0, 0, rt.Bounds, renderTargetSave.PaintAlpha / 255f, CanvasImageInterpolation.Linear, canvasComposite);
            }

            CurrentDrawingSession.Flush();
        }

        public void DrawBitmap(CanvasBitmap bitmap, Rect src, Rect dst, Paint paint)
        {
            UpdateDrawingSessionWithFlags(paint.Flags);
            CurrentDrawingSession.Transform = GetCurrentTransform();

            var canvasComposite = CanvasComposite.SourceOver;

            // TODO paint.ColorFilter
            // if (paint.ColorFilter is PorterDuffColorFilter porterDuffColorFilter)
            //    canvasComposite = PorterDuff.ToCanvasComposite(porterDuffColorFilter.Mode);
            CurrentDrawingSession.DrawImage(bitmap, dst, src, paint.Alpha / 255f, CanvasImageInterpolation.NearestNeighbor, canvasComposite);
        }

        public void GetClipBounds(out Rect bounds)
        {
            RectExt.Set(ref bounds, _currentClip.X, _currentClip.Y, _currentClip.Width, _currentClip.Height);
        }

        public void Clear(Color color)
        {
            UpdateDrawingSessionWithFlags(0);

            CurrentDrawingSession.Clear(color);

            _matrixSaves.Clear();
            _flagSaves.Clear();
            _clipSaves.Clear();
        }

        private void UpdateDrawingSessionWithFlags(int flags)
        {
            CurrentDrawingSession.Antialiasing = (flags & Paint.AntiAliasFlag) == Paint.AntiAliasFlag
                ? CanvasAntialiasing.Antialiased
                : CanvasAntialiasing.Aliased;
        }

        public void Translate(float dx, float dy)
        {
            _matrix = MatrixExt.PreTranslate(_matrix, dx, dy);
        }

        public void Scale(float sx, float sy, float px, float py)
        {
            _matrix = MatrixExt.PreScale(_matrix, sx, sy, px, py);
        }

        public void SetMatrix(Matrix3X3 matrix)
        {
            _matrix.Set(matrix);
        }

        public Rect DrawText(char character, Paint paint)
        {
            var gradient = paint.Shader as Gradient;
            var brush = gradient != null ? gradient.GetBrush(_device, paint.Alpha) : new CanvasSolidColorBrush(_device, paint.Color);
            brush = paint.ColorFilter?.Apply(_device, brush) ?? brush;

            UpdateDrawingSessionWithFlags(paint.Flags);
            CurrentDrawingSession.Transform = GetCurrentTransform();

            var text = new string(character, 1);

            var textFormat = new CanvasTextFormat
            {
                FontSize = paint.TextSize,
                FontFamily = paint.Typeface.FontFamily,
                FontStyle = paint.Typeface.Style,
                FontWeight = paint.Typeface.Weight,
                VerticalAlignment = CanvasVerticalAlignment.Center,
                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                LineSpacingBaseline = 0,
                LineSpacing = 0
            };
            var textLayout = new CanvasTextLayout(CurrentDrawingSession, text, textFormat, 0.0f, 0.0f);
            CurrentDrawingSession.DrawText(text, 0, 0, brush, textFormat);

            if (gradient == null || paint.ColorFilter != null)
            {
                brush.Dispose();
            }

            return textLayout.LayoutBounds;
        }

        private void Dispose(bool disposing)
        {
            foreach (var renderTarget in _renderTargets)
            {
                renderTarget.Value.Dispose();
            }

            _renderTargets.Clear();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BitmapCanvas()
        {
            Dispose(false);
        }
    }
}