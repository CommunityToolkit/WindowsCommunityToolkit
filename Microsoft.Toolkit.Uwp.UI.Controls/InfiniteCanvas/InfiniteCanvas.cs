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
        const float LARGE_CANVAS_WIDTH = 1 << 21;
        const float LARGE_CANVAS_HEIGHT = 1 << 21;

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

            OutputGrid.Width = LARGE_CANVAS_WIDTH;
            OutputGrid.Height = LARGE_CANVAS_HEIGHT;
        }

        void OnCanvasControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.renderTarget?.Dispose();
            this.renderTarget = null;
        }

        void OnStrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            this.wetInkStrokes = this.inkSync.BeginDry();

            _canvasOne.DrawLine(this.wetInkStrokes);
            //this.strokeContainer.AddStrokes(args.Strokes);

            this.inkSync.EndDry();
            //this.canvasControl.Invalidate();
        }

        private void InkScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                _canvasOne.UpdateZoomFactor(inkScrollViewer.ZoomFactor);
            }
        }
    }
}
