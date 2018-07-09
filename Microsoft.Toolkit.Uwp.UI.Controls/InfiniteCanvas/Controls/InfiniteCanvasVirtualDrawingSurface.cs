// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Graphics;
using Windows.Graphics.DirectX;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The virtual Drawing surface renderer used to render the ink and text.
    /// </summary>
    internal partial class InfiniteCanvasVirtualDrawingSurface : Panel
    {
        private Compositor _compositor;
        private CanvasDevice _win2DDevice;
        private CompositionGraphicsDevice _comositionGraphicsDevice;
        private SpriteVisual _myDrawingVisual;
        private CompositionVirtualDrawingSurface _drawingSurface;
        private CompositionSurfaceBrush _surfaceBrush;

        public InfiniteCanvasVirtualDrawingSurface()
        {
            InitializeComposition();
            SizeChanged += TheSurface_SizeChanged;
        }

        private void TheSurface_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _myDrawingVisual.Size = new Vector2((float)ActualWidth, (float)ActualHeight);
        }

        public void InitializeComposition()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _win2DDevice = CanvasDevice.GetSharedDevice();
            _comositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, _win2DDevice);
            _myDrawingVisual = _compositor.CreateSpriteVisual();
            ElementCompositionPreview.SetElementChildVisual(this, _myDrawingVisual);
        }

        public void ConfigureSpriteVisual(double width, double height)
        {
            var size = new SizeInt32
            {
                Height = (int)width,
                Width = (int)height
            };

            _drawingSurface = _comositionGraphicsDevice.CreateVirtualDrawingSurface(
                size,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            _surfaceBrush = _compositor.CreateSurfaceBrush(_drawingSurface);
            _surfaceBrush.Stretch = CompositionStretch.None;
            _surfaceBrush.HorizontalAlignmentRatio = 0;
            _surfaceBrush.VerticalAlignmentRatio = 0;
            _surfaceBrush.TransformMatrix = Matrix3x2.CreateTranslation(0, 0);

            _myDrawingVisual.Brush = _surfaceBrush;
            _surfaceBrush.Offset = new Vector2(0, 0);
        }
    }
}
