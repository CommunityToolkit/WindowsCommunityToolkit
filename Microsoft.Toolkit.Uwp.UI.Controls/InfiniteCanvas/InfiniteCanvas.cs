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
using System.Diagnostics;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Infinite Canvas
    /// </summary>
    public class InfiniteCanvas : Control
    {
        InkCanvas _inkCanvas;
        VirtualDrawingSurface _canvasOne;
        private Grid CanvasContainer;

        InkStrokeContainer strokeContainer;
        CanvasRenderTarget renderTarget;
        IReadOnlyList<InkStroke> wetInkStrokes;
        InkSynchronizer inkSync;
        internal const float LargeCanvasWidth = 1 << 14;
        internal const float LargeCanvasHeight = 1 << 14;

        public InfiniteCanvas()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvas);
        }

        protected override void OnApplyTemplate()
        {

            _canvasOne = (VirtualDrawingSurface)GetTemplateChild("canvasOne");
            CanvasContainer = (Grid)GetTemplateChild("CanvasContainer");
            OutputGrid = (Grid)GetTemplateChild("OutputGrid");
            inkScrollViewer = (ScrollViewer)GetTemplateChild("inkScrollViewer");

            //inkScrollViewer.Clip
            //inkScrollViewer.ExtentHeight
            //inkScrollViewer.ExtentWidth
            //inkScrollViewer.HorizontalOffset
            //VerticalOffset
            //inkScrollViewer.ZoomFactor
            //inkScrollViewer.ScrollableHeight
            //inkScrollViewer.ViewportHeight
            //inkScrollViewer.ViewportWidth

            canToolBar = (InkToolbar)GetTemplateChild("canToolBar");

            _inkCanvas = (InkCanvas)GetTemplateChild("inkCanvas");
            //var enableButton = (Button)GetTemplateChild("EnableDisableButton");
            //enableButton.Click += EnableButton_Click;
            canToolBar.TargetInkCanvas = _inkCanvas;

            MainPage_Loaded();
            base.OnApplyTemplate();
        }

        public InkToolbar canToolBar { get; set; }

        public Grid OutputGrid { get; set; }
        public ScrollViewer inkScrollViewer { get; set; }

        private void EnableButton_Click(object sender, RoutedEventArgs e)
        {
            _inkCanvas.Visibility = _inkCanvas.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MainPage_Loaded()
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;

            this.inkSync = this._inkCanvas.InkPresenter.ActivateCustomDrying();
            this._inkCanvas.InkPresenter.StrokesCollected += OnStrokesCollected;



            //this.canvasControl.SizeChanged += OnCanvasControlSizeChanged; ;

            this.strokeContainer = new InkStrokeContainer();


            inkScrollViewer.MaxZoomFactor = 4.0f;
            inkScrollViewer.MinZoomFactor = 0.25f;
            inkScrollViewer.ViewChanged += InkScrollViewer_ViewChanged;
            inkScrollViewer.SizeChanged += InkScrollViewer_SizeChanged;

            OutputGrid.Width = LargeCanvasWidth;
            OutputGrid.Height = LargeCanvasHeight;
        }

        private void ReDrawCanvas()
        {
            _canvasOne.ReDraw(ViewPort);
        }

        private void InkScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReDrawCanvas();
        }

        void OnCanvasControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.renderTarget?.Dispose();
            this.renderTarget = null;
        }

        void OnStrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            IReadOnlyList<InkStroke> strokes = this.inkSync.BeginDry();
            var inkDrawable = new InkDrawable(strokes);
            _canvasOne.AddDrawable(inkDrawable);
            this.inkSync.EndDry();

            ReDrawCanvas();
        }

        private void InkScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                _canvasOne.UpdateZoomFactor(inkScrollViewer.ZoomFactor);
                ReDrawCanvas();
            }
        }

        private Rect ViewPort => new Rect(inkScrollViewer.HorizontalOffset, inkScrollViewer.VerticalOffset, inkScrollViewer.ViewportWidth / inkScrollViewer.ZoomFactor, inkScrollViewer.ViewportHeight / inkScrollViewer.ZoomFactor);
    }
}
