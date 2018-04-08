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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Infinite Canvas
    /// </summary>
    public partial class InfiniteCanvas : Control
    {
        InkCanvas _inkCanvas;
        VirtualDrawingSurface _canvasOne;
        IReadOnlyList<InkStroke> wetInkStrokes;
        InkSynchronizer inkSync;

        internal const float LargeCanvasWidthHeight = 1 << 21;

        public InfiniteCanvas()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvas);
        }

        private InkToolbarCustomToolButton _enableTextButton;
        private InkToolbarCustomToggleButton _enableTouchInkingButton;
        private InfiniteCanvasTextBox _canvasTextBox;
        private StackPanel _canvasTextBoxTools;
        private ColorPicker _canvasTextBoxColorPicker;

        private TextBox _canvasTextBoxFontSizeTextBox;
        private ToggleButton _canvasTextBoxItlaicButton;
        private ToggleButton _canvasTextBoxBoldButton;

        protected override void OnApplyTemplate()
        {
            _canvasTextBoxTools = (StackPanel)GetTemplateChild("CanvasTextBoxTools");

            _canvasTextBoxColorPicker = (ColorPicker)GetTemplateChild("CanvasTextBoxColorPicker");
            _canvasTextBoxFontSizeTextBox = (TextBox)GetTemplateChild("CanvasTextBoxFontSizeTextBox");
            _canvasTextBoxItlaicButton = (ToggleButton)GetTemplateChild("CanvasTextBoxItlaicButton");
            _canvasTextBoxBoldButton = (ToggleButton)GetTemplateChild("CanvasTextBoxBoldButton");

            _canvasTextBoxFontSizeTextBox.TextChanged += _canvasTextBoxFontSizeTextBox_TextChanged;
            _canvasTextBoxItlaicButton.Checked += _canvasTextBoxItlaicButton_Checked;
            _canvasTextBoxBoldButton.Checked += _canvasTextBoxBoldButton_Checked;

            _canvasTextBoxColorPicker.ColorChanged += _canvasTextBoxColorPicker_ColorChanged;

            _canvasOne = (VirtualDrawingSurface)GetTemplateChild("canvasOne");
            OutputGrid = (Canvas)GetTemplateChild("OutputGrid");

            inkScrollViewer = (ScrollViewer)GetTemplateChild("inkScrollViewer");
            var eraseAllButton = (Button)GetTemplateChild("EraseAllButton");

            _canvasTextBox = (InfiniteCanvasTextBox)GetTemplateChild("CanvasTextBox");

            _enableTextButton = (InkToolbarCustomToolButton)GetTemplateChild("EnableTextButton");

            _enableTouchInkingButton = (InkToolbarCustomToggleButton)GetTemplateChild("EnableTouchInkingButton");

            _enableTouchInkingButton.Checked += _enableTouchInkingButton_Checked;
            _enableTouchInkingButton.Unchecked += _enableTouchInkingButton_Unchecked;

            _enableTextButton.Checked += _enableTextButton_Checked;
            _enableTextButton.Unchecked += _enableTextButton_Unchecked;
            eraseAllButton.Click += EraseAllButton_Click;
            canToolBar = (InkToolbar)GetTemplateChild("canToolBar");

            _inkCanvas = (InkCanvas)GetTemplateChild("inkCanvas");
            inkScrollViewer.PointerPressed += InkScrollViewer_PointerPressed;
            inkScrollViewer.PreviewKeyDown += InkScrollViewer_PreviewKeyDown;
            _canvasTextBox.TextChanged += _canvasTextBox_TextChanged;
            _canvasTextBox.SizeChanged += _canvasTextBox_SizeChanged;

            MainPage_Loaded();
            base.OnApplyTemplate();
        }

        private void _enableTouchInkingButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
        }

        private void _enableTouchInkingButton_Checked(object sender, RoutedEventArgs e)
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;
        }

        private void _enableTextButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _canvasTextBox.Visibility = Visibility.Collapsed;
            _inkCanvas.Visibility = Visibility.Visible;
            _canvasTextBoxTools.Visibility = Visibility.Collapsed;
        }

        private void _enableTextButton_Checked(object sender, RoutedEventArgs e)
        {
            _inkCanvas.Visibility = Visibility.Collapsed;
            _canvasTextBoxTools.Visibility = Visibility.Visible;
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

            _canvasTextBox.UpdateFontSize(TextFontSize);
        }

        private async void Current_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            // work around to virtual drawing surface bug.
            await Task.Delay(1000);
            _canvasOne.ReDraw(ViewPort);
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (canToolBar.ActiveTool == canToolBar.GetToolButton(InkToolbarTool.Eraser))
            {
                _canvasOne.Erase(args.CurrentPoint.Position, ViewPort, inkScrollViewer.ZoomFactor);
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
                ClearTextBoxValue();
                ReDrawCanvas();
            }
        }

        private Rect ViewPort => new Rect(inkScrollViewer.HorizontalOffset / inkScrollViewer.ZoomFactor, inkScrollViewer.VerticalOffset / inkScrollViewer.ZoomFactor, inkScrollViewer.ViewportWidth / inkScrollViewer.ZoomFactor, inkScrollViewer.ViewportHeight / inkScrollViewer.ZoomFactor);
    }
}
