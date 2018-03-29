using System;
using System.Collections.Generic;
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
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class VirtualDrawingSurface : Panel, IInteractionTrackerOwner
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

        private const int TILESIZE = 250;
        Random randonGen = new Random();

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
            //DrawTile(new Rect(0, 0, 200, 200), 0, 0);
            //DrawTile(new Rect(200, 200, 200, 200), 1, 1);

            //            drawingSurface.Trim(
            //                new[] { new RectInt32
            //                { X = 0, Y = 0, Width = 50, Height = 50 }
            //}
            //                );
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
                Height = 10000,
                Width = 10000
            };

            this.drawingSurface = comositionGraphicsDevice.CreateVirtualDrawingSurface(size,
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

            this.tracker = InteractionTracker.CreateWithOwner(this.compositor, this);
            this.tracker.InteractionSources.Add(this.interactionSource);

            this.moveSurfaceExpressionAnimation = this.compositor.CreateExpressionAnimation("-tracker.Position.X");
            this.moveSurfaceExpressionAnimation.SetReferenceParameter("tracker", this.tracker);

            this.moveSurfaceUpDownExpressionAnimation = this.compositor.CreateExpressionAnimation("-tracker.Position.Y");
            this.moveSurfaceUpDownExpressionAnimation.SetReferenceParameter("tracker", this.tracker);

            this.scaleSurfaceUpDownExpressionAnimation = this.compositor.CreateExpressionAnimation("tracker.Scale");
            this.scaleSurfaceUpDownExpressionAnimation.SetReferenceParameter("tracker", this.tracker);

            this.tracker.MinPosition = new Vector3(0, 0, 0);
            //TODO: use same consts as tilemanager object
            this.tracker.MaxPosition = new Vector3(10000, 10000, 0);

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
            Color randomColor = Color.FromArgb((byte)255, (byte)randonGen.Next(255), (byte)randonGen.Next(255), (byte)randonGen.Next(255));
            using (CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface, rect))
            {
                drawingSession.Clear(randomColor);
                CanvasTextFormat tf = new CanvasTextFormat() { FontSize = 72 };
                drawingSession.DrawText($"{tileColumn},{tileRow}", new Vector2(50, 50), Colors.Green, tf);
            }
        }

        public void Trim(Rect trimRect)
        {
            drawingSurface.Trim(new RectInt32[] { new RectInt32 { X = (int)trimRect.X, Y = (int)trimRect.Y, Width = (int)trimRect.Width, Height = (int)trimRect.Height } });
        }

        public void CustomAnimationStateEntered(InteractionTracker sender, InteractionTrackerCustomAnimationStateEnteredArgs args)
        {

        }

        public void IdleStateEntered(InteractionTracker sender, InteractionTrackerIdleStateEnteredArgs args)
        {

        }

        public void InertiaStateEntered(InteractionTracker sender, InteractionTrackerInertiaStateEnteredArgs args)
        {

        }

        public void InteractingStateEntered(InteractionTracker sender, InteractionTrackerInteractingStateEnteredArgs args)
        {

        }

        public void RequestIgnored(InteractionTracker sender, InteractionTrackerRequestIgnoredArgs args)
        {

        }

        public void ValuesChanged(InteractionTracker sender, InteractionTrackerValuesChangedArgs args)
        {

        }

        List<InkStroke> list = new List<InkStroke>();

        public void DrawLine(IReadOnlyList<InkStroke> inkes)
        {

            list.AddRange(inkes);

            //Debug.WriteLine($"{tracker.Position}");


            // full screen but only record the last element as every time we draw it clear the old drawings
            using (
                var drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                CanvasTextFormat tf = new CanvasTextFormat() { FontSize = 72 };
                //drawingSession.DrawText($"hopa", 50, 50, Colors.Green, tf);

                float xLoc = 100.0f;
                float yLoc = 100.0f;
                //CanvasTextFormat format = new CanvasTextFormat
                //{
                //    FontSize = 30.0f,
                //    WordWrapping = CanvasWordWrapping.NoWrap
                //};
                //CanvasTextLayout textLayout = new CanvasTextLayout(drawingSession, "Hello World!", format, 0.0f, 0.0f);
                //Rect theRectYouAreLookingFor = new Rect(xLoc + textLayout.DrawBounds.X, yLoc + textLayout.DrawBounds.Y,
                //    textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);
                //drawingSession.DrawRectangle(theRectYouAreLookingFor, Colors.Green, 1.0f);
                //drawingSession.DrawTextLayout(textLayout, xLoc, yLoc, Colors.Yellow);

                //DrawTile(new Rect(0, 0, 200, 200), 0, 0);
                //DrawTile(new Rect(200, 200, 200, 200), 1, 1);

                drawingSession.DrawInk(list);
            }
        }

        private string sofar = string.Empty;
        public void DrawString(string c)
        {
            sofar += c;
            using (
                var drawingSession = CanvasComposition.CreateDrawingSession(drawingSurface,
                    new Rect(0, 0, ActualWidth, ActualHeight)))
            {
                CanvasTextFormat tf = new CanvasTextFormat() { FontSize = 72 };

                float xLoc = 100.0f;
                float yLoc = 100.0f;
                CanvasTextFormat format = new CanvasTextFormat
                {
                    FontSize = 30.0f,
                    WordWrapping = CanvasWordWrapping.NoWrap
                };
                CanvasTextLayout textLayout = new CanvasTextLayout(drawingSession, sofar, format, 0.0f,
                    0.0f);
                Rect theRectYouAreLookingFor = new Rect(xLoc + textLayout.DrawBounds.X,
                    yLoc + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);
                drawingSession.DrawRectangle(theRectYouAreLookingFor, Colors.Green, 1.0f);
                drawingSession.DrawTextLayout(textLayout, xLoc, yLoc, Colors.Yellow);

                //drawingSession.DrawText()

                drawingSession.DrawInk(list);
            }
        }

        public void UpdateZoomFactor(float zoomFactor)
        {


            startAnimation(surfaceBrush, zoomFactor);


        }
    }
}
