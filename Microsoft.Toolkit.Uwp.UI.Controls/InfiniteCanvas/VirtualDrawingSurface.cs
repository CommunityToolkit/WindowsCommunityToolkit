using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Foundation;
using Windows.Graphics;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Interactions;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class VirtualDrawingSurface : Panel
    {
        private Compositor compositor;
        private CanvasDevice win2dDevice;
        private CompositionGraphicsDevice comositionGraphicsDevice;
        private SpriteVisual myDrawingVisual;
        private CompositionVirtualDrawingSurface drawingSurface;
        private CompositionSurfaceBrush surfaceBrush;
        private InteractionTracker tracker;
        private VisualInteractionSource interactionSource;
        private CompositionPropertySet animatingPropset;
        private ExpressionAnimation animateMatrix;
        private ExpressionAnimation moveSurfaceExpressionAnimation;
        private ExpressionAnimation moveSurfaceUpDownExpressionAnimation;
        private ExpressionAnimation scaleSurfaceUpDownExpressionAnimation;

        public VirtualDrawingSurface()
        {
            InitializeComposition();
            ConfigureSpriteVisual();
            ConfigureInteraction();
            startAnimation(surfaceBrush);
            Loaded += MainPage_Loaded;
            this.SizeChanged += TheSurface_SizeChanged;
            this.PointerPressed += InfiniteCanvas_PointerPressed;
            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            //var key = args.VirtualKey.ToString();
            //args.Handled = true;
            //DrawString(key);
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            try
            {
                interactionSource.TryRedirectForManipulation(args.CurrentPoint);
            }
            catch
            {

            }
        }

        private void InfiniteCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            try
            {
                var currentPoint = e.GetCurrentPoint(this);
                interactionSource.TryRedirectForManipulation(currentPoint);
            }
            catch
            {

            }
        }

        private void TheSurface_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myDrawingVisual.Size = new Vector2((float)ActualWidth, (float)ActualHeight);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void InitializeComposition()
        {
            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            win2dDevice = CanvasDevice.GetSharedDevice();
            comositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, win2dDevice);
            myDrawingVisual = compositor.CreateSpriteVisual();
            ElementCompositionPreview.SetElementChildVisual(this, myDrawingVisual);
        }

        public void ConfigureSpriteVisual()
        {
            var size = new SizeInt32
            {
                Height = (int)InfiniteCanvas.LargeCanvasWidthHeight,
                Width = (int)InfiniteCanvas.LargeCanvasWidthHeight
            };

            this.drawingSurface = comositionGraphicsDevice.CreateVirtualDrawingSurface(
                size,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                DirectXAlphaMode.Premultiplied);

            this.surfaceBrush = compositor.CreateSurfaceBrush(drawingSurface);
            this.surfaceBrush.Stretch = CompositionStretch.None;
            this.surfaceBrush.HorizontalAlignmentRatio = 0;
            this.surfaceBrush.VerticalAlignmentRatio = 0;
            this.surfaceBrush.TransformMatrix = Matrix3x2.CreateTranslation(20.0f, 20.0f);

            this.myDrawingVisual.Brush = surfaceBrush;
            this.surfaceBrush.Offset = new Vector2(0, 0);
        }

        public void ConfigureInteraction()
        {
            this.interactionSource = VisualInteractionSource.Create(myDrawingVisual);
            this.interactionSource.PositionXSourceMode = InteractionSourceMode.EnabledWithInertia;
            this.interactionSource.PositionYSourceMode = InteractionSourceMode.EnabledWithInertia;

            this.interactionSource.ScaleSourceMode = InteractionSourceMode.EnabledWithInertia;

            this.tracker = InteractionTracker.Create(this.compositor);
            this.tracker.InteractionSources.Add(this.interactionSource);

            this.moveSurfaceExpressionAnimation = this.compositor.CreateExpressionAnimation("-tracker.Position.X");
            this.moveSurfaceExpressionAnimation.SetReferenceParameter("tracker", this.tracker);

            this.moveSurfaceUpDownExpressionAnimation = this.compositor.CreateExpressionAnimation("-tracker.Position.Y");
            this.moveSurfaceUpDownExpressionAnimation.SetReferenceParameter("tracker", this.tracker);

            this.scaleSurfaceUpDownExpressionAnimation = this.compositor.CreateExpressionAnimation("tracker.Scale");
            this.scaleSurfaceUpDownExpressionAnimation.SetReferenceParameter("tracker", this.tracker);

            this.tracker.MinPosition = new Vector3(0, 0, 0);
            //TODO: use same consts as tilemanager object
            this.tracker.MaxPosition = new Vector3(InfiniteCanvas.LargeCanvasWidthHeight, InfiniteCanvas.LargeCanvasWidthHeight, 0);

            this.tracker.MinScale = .25f;
            this.tracker.MaxScale = 4f;

        }

        private void startAnimation(CompositionSurfaceBrush brush, float scale = 1f)
        {
            animatingPropset = compositor.CreatePropertySet();
            animatingPropset.InsertScalar("xcoord", 1.0f);
            animatingPropset.StartAnimation("xcoord", moveSurfaceExpressionAnimation);

            animatingPropset.InsertScalar("ycoord", 1.0f);
            animatingPropset.StartAnimation("ycoord", moveSurfaceUpDownExpressionAnimation);

            animatingPropset.InsertScalar("scale", scale);
            animatingPropset.StartAnimation("scale", scaleSurfaceUpDownExpressionAnimation);

            animateMatrix = compositor.CreateExpressionAnimation("Matrix3x2(props.scale, 0.0, 0.0, props.scale, props.xcoord, props.ycoord)");
            animateMatrix.SetReferenceParameter("props", animatingPropset);

            brush.StartAnimation(nameof(brush.TransformMatrix), animateMatrix);
        }

        public void DrawTile(Rect rect, int tileRow, int tileColumn)
        {

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, rect))
            {
                //drawingSession.Clear(randomColor);
                CanvasTextFormat tf = new CanvasTextFormat() { FontSize = 72 };
                drawingSession.DrawText($"{tileColumn},{tileRow}", new Vector2(50, 50), Colors.Green, tf);
            }
        }

        public void Trim(Rect trimRect)
        {
            drawingSurface.Trim(new RectInt32[] { new RectInt32 { X = (int)trimRect.X, Y = (int)trimRect.Y, Width = (int)trimRect.Width, Height = (int)trimRect.Height } });
        }

        List<IReadOnlyList<InkStroke>> list = new List<IReadOnlyList<InkStroke>>();

        public void DrawLine(IReadOnlyList<InkStroke> inkes)
        {
            list.Add(inkes);
        }

        private string sofar = string.Empty;
        public void DrawString(string c, CanvasDrawingSession drawingSession)
        {
            sofar = c;

            float xLoc = 100.0f;
            float yLoc = 100.0f;
            CanvasTextFormat format = new CanvasTextFormat
            {
                FontSize = 30.0f,
                FontStyle = FontStyle.Italic,
                FontWeight = FontWeights.Bold,
                WordWrapping = CanvasWordWrapping.NoWrap
            };

            CanvasTextLayout textLayout = new CanvasTextLayout(drawingSession, sofar, format, 0.0f,
                0.0f);
            Rect theRectYouAreLookingFor = new Rect(xLoc + textLayout.DrawBounds.X,
                yLoc + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);

            drawingSession.DrawRectangle(theRectYouAreLookingFor, Colors.Gray, 1.0f);
            drawingSession.DrawTextLayout(textLayout, xLoc, yLoc, Colors.Black);

        }

        public void UpdateZoomFactor(float zoomFactor)
        {
            startAnimation(surfaceBrush, zoomFactor);
        }

        public Color Background { get; set; } = Colors.White;

        public void ReDraw(Rect viewPort)
        {
            var visibleList = new List<IDrawable>();
            foreach (var drawable in _drawableList)
            {
                if (drawable.IsVisible(viewPort))
                {
                    visibleList.Add(drawable);
                }
            }

            Rect toDraw;
            var first = visibleList.FirstOrDefault();
            if (first != null)
            {
                double top = first.Bounds.Top,
                    bottom = first.Bounds.Bottom,
                    left = first.Bounds.Left,
                    right = first.Bounds.Right;

                for (var index = 1; index < visibleList.Count; index++)
                {
                    var stroke = visibleList[index];
                    bottom = Math.Max(stroke.Bounds.Bottom, bottom);
                    right = Math.Max(stroke.Bounds.Right, right);
                    top = Math.Min(stroke.Bounds.Top, top);
                    left = Math.Min(stroke.Bounds.Left, left);
                }

                toDraw = new Rect(Math.Max(left, 0), Math.Max(top, 0), Math.Max(right - left, 0), Math.Max(bottom - top, 0));

                toDraw.Union(viewPort);
            }
            else
            {
                toDraw = viewPort;
            }

            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, toDraw))
            {
                drawingSession.Clear(Background);
                foreach (var drawable in visibleList)
                {
                    drawable.Draw(drawingSession, toDraw);
                }
            }
        }

        private readonly List<IDrawable> _drawableList = new List<IDrawable>();

        internal void AddDrawable(IDrawable inkDrawable)
        {
            _drawableList.Add(inkDrawable);
        }

        public void Erase(Point point, Rect viewPort)
        {
            for (var i = _drawableList.Count - 1; i >= 0; i--)
            {
                var drawable = _drawableList[i];
                Debug.WriteLine($"{drawable.Bounds.Contains(point)}, {drawable.Bounds}, {point}");
                if (drawable.IsActive && drawable is InkDrawable inkDrawable && drawable.Bounds.Contains(point))
                {
                    foreach (var stroke in inkDrawable.Strokes)
                    {
                        if (stroke.BoundingRect.Contains(point))
                        {
                            _drawableList.RemoveAt(i);
                            ReDraw(viewPort);
                            return;
                        }
                    }
                }
            }
        }

        public void ClearAll(Rect viewPort)
        {
            _drawableList.Clear();
            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, viewPort))
            {
                drawingSession.Clear(Background);
            }
        }
    }
}
