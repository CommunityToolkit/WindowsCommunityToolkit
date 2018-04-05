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
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Enums;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Infinite Canvas
    /// </summary>
    public partial class InfiniteCanvas : Control
    {
        InkCanvas _inkCanvas;
        VirtualDrawingSurface _canvasOne;
        private Grid CanvasContainer;

        InkStrokeContainer strokeContainer;
        CanvasRenderTarget renderTarget;
        IReadOnlyList<InkStroke> wetInkStrokes;
        InkSynchronizer inkSync;

        internal const float LargeCanvasWidthHeight = 1 << 21;

        public InfiniteCanvas()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvas);
        }

        private InkToolbarCustomToolButton _enableTextButton;
        private InfiniteCanvasTextBox _canvasTextBox;
        protected override void OnApplyTemplate()
        {

            _canvasOne = (VirtualDrawingSurface)GetTemplateChild("canvasOne");
            CanvasContainer = (Grid)GetTemplateChild("CanvasContainer");
            OutputGrid = (Canvas)GetTemplateChild("OutputGrid");

            inkScrollViewer = (ScrollViewer)GetTemplateChild("inkScrollViewer");
            var eraseAllButton = (InkToolbarCustomToolButton)GetTemplateChild("EraseAllButton");

            _canvasTextBox = (InfiniteCanvasTextBox)GetTemplateChild("CanvasTextBox");

            _enableTextButton = (InkToolbarCustomToolButton)GetTemplateChild("EnableTextButton");

            _enableTextButton.Checked += _enableTextButton_Checked;
            _enableTextButton.Unchecked += _enableTextButton_Unchecked;
            eraseAllButton.Click += EraseAllButton_Click;

            canToolBar = (InkToolbar)GetTemplateChild("canToolBar");

            _inkCanvas = (InkCanvas)GetTemplateChild("inkCanvas");
            inkScrollViewer.PointerPressed += InkScrollViewer_PointerPressed;

            _canvasTextBox.TextChanged += _canvasTextBox_TextChanged;

            MainPage_Loaded();
            base.OnApplyTemplate();
        }

        public float FontSize { get; set; } = 22;

        private TextDrawable _selectedTextDrawable;

        private void _canvasTextBox_TextChanged(object sender, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (_selectedTextDrawable != null)
            {
                _selectedTextDrawable.Text = text;
                _canvasOne.ReDraw(ViewPort);
                return;
            }

            _selectedTextDrawable = new TextDrawable(
                _lastInputPoint.Y, _lastInputPoint.X,
                FontSize,
                _canvasTextBox.GetEditZoneHeight(),
                _canvasTextBox.GetEditZoneWidth(),
                text);

            _canvasOne.AddDrawable(_selectedTextDrawable);
            _canvasOne.ReDraw(ViewPort);
        }

        Point _lastInputPoint;

        private void InkScrollViewer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_enableTextButton.IsChecked ?? false)
            {
                ClearTextBoxValue();

                var point = e.GetCurrentPoint(inkScrollViewer);
                _canvasTextBox.Visibility = Visibility.Visible;

                _lastInputPoint = new Point((point.Position.X + inkScrollViewer.HorizontalOffset) / inkScrollViewer.ZoomFactor, (point.Position.Y + inkScrollViewer.VerticalOffset) / inkScrollViewer.ZoomFactor);

                Canvas.SetLeft(_canvasTextBox, _lastInputPoint.X);
                Canvas.SetTop(_canvasTextBox, _lastInputPoint.Y);
            }
        }

        private void _enableTextButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _canvasTextBox.Visibility = Visibility.Collapsed;
            _inkCanvas.Visibility = Visibility.Visible;
        }

        private void _enableTextButton_Checked(object sender, RoutedEventArgs e)
        {
            _inkCanvas.Visibility = Visibility.Collapsed;
        }

        private void EraseAllButton_Click(object sender, RoutedEventArgs e)
        {
            _canvasOne.ClearAll(ViewPort);
        }

        public InkToolbar canToolBar { get; set; }

        public Canvas OutputGrid { get; set; }
        public ScrollViewer inkScrollViewer { get; set; }

        private void MainPage_Loaded()
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;

            this.inkSync = this._inkCanvas.InkPresenter.ActivateCustomDrying();
            this._inkCanvas.InkPresenter.StrokesCollected += OnStrokesCollected;
            this._inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;

            this.strokeContainer = new InkStrokeContainer();


            inkScrollViewer.MaxZoomFactor = 4.0f;
            inkScrollViewer.MinZoomFactor = 0.25f;
            inkScrollViewer.ViewChanged += InkScrollViewer_ViewChanged;
            inkScrollViewer.SizeChanged += InkScrollViewer_SizeChanged;

            OutputGrid.Width = LargeCanvasWidthHeight;
            OutputGrid.Height = LargeCanvasWidthHeight;
            _inkCanvas.Width = LargeCanvasWidthHeight;
            _inkCanvas.Height = LargeCanvasWidthHeight;
            _canvasOne.Width = LargeCanvasWidthHeight;
            _canvasOne.Height = LargeCanvasWidthHeight;

            Application.Current.LeavingBackground += Current_LeavingBackground;

            Canvas.SetLeft(_canvasTextBox, 0);
            Canvas.SetTop(_canvasTextBox, 0);

            _canvasTextBox.FontSize = FontSize;
            // _canvasTextBox.PointerWheelChanged += _canvasTextBox_PointerWheelChanged;
        }

        private async void Current_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            // work around to virtual drawing surface bug.
            await Task.Delay(1000);
            _canvasOne.ReDraw(ViewPort);
        }

        private void _canvasTextBox_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _canvasTextBox.Visibility = Visibility.Collapsed;
            _inkCanvas.Visibility = Visibility.Visible;
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (canToolBar.ActiveTool == canToolBar.GetToolButton(InkToolbarTool.Eraser))
            {
                _canvasOne.Erase(args.CurrentPoint.Position, ViewPort);
            }
        }

        private void ReDrawCanvas()
        {
            _canvasOne.ReDraw(ViewPort);
        }

        private void InkScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClearTextBoxValue();
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
            Debug.WriteLine("Scroll");
            if (!e.IsIntermediate)
            {
                ClearTextBoxValue();
                _canvasOne.UpdateZoomFactor(inkScrollViewer.ZoomFactor);
                ReDrawCanvas();
            }
        }

        private void ClearTextBoxValue()
        {
            _selectedTextDrawable = null;
            _canvasTextBox.Visibility = Visibility.Collapsed;
            _canvasTextBox.Clear();
        }

        private Rect ViewPort => new Rect(inkScrollViewer.HorizontalOffset / inkScrollViewer.ZoomFactor, inkScrollViewer.VerticalOffset / inkScrollViewer.ZoomFactor, inkScrollViewer.ViewportWidth / inkScrollViewer.ZoomFactor, inkScrollViewer.ViewportHeight / inkScrollViewer.ZoomFactor);
    }
}
