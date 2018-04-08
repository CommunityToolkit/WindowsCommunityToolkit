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
        private InkCanvas _inkCanvas;
        private InfiniteCanvasVirtualDrawingSurface _drawingSurfaceRenderer;
        private InkSynchronizer _inkSync;
        private InkToolbarCustomToolButton _enableTextButton;
        private InkToolbarCustomToggleButton _enableTouchInkingButton;
        private InfiniteCanvasTextBox _canvasTextBox;
        private StackPanel _canvasTextBoxTools;
        private ColorPicker _canvasTextBoxColorPicker;

        private TextBox _canvasTextBoxFontSizeTextBox;
        private ToggleButton _canvasTextBoxItlaicButton;
        private ToggleButton _canvasTextBoxBoldButton;
        private Button _undoButton;
        private Button _redoButton;

        private InkToolbar _inkCanvasToolBar;
        private Canvas _mainContainer;
        private ScrollViewer _infiniteCanvasScrollViewer;

        // TODO as property
        internal const float LargeCanvasWidthHeight = 1 << 21;

        private Rect ViewPort => new Rect(_infiniteCanvasScrollViewer.HorizontalOffset / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.VerticalOffset / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.ViewportWidth / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.ViewportHeight / _infiniteCanvasScrollViewer.ZoomFactor);

        public InfiniteCanvas()
        {
            this.DefaultStyleKey = typeof(InfiniteCanvas);
        }

        protected override void OnApplyTemplate()
        {
            _canvasTextBoxTools = (StackPanel)GetTemplateChild("CanvasTextBoxTools");
            _canvasTextBoxColorPicker = (ColorPicker)GetTemplateChild("CanvasTextBoxColorPicker");
            _canvasTextBoxFontSizeTextBox = (TextBox)GetTemplateChild("CanvasTextBoxFontSizeTextBox");
            _canvasTextBoxItlaicButton = (ToggleButton)GetTemplateChild("CanvasTextBoxItlaicButton");
            _canvasTextBoxBoldButton = (ToggleButton)GetTemplateChild("CanvasTextBoxBoldButton");

            _drawingSurfaceRenderer = (InfiniteCanvasVirtualDrawingSurface)GetTemplateChild("DrawingSurfaceRenderer");
            _mainContainer = (Canvas)GetTemplateChild("MainContainer");

            _infiniteCanvasScrollViewer = (ScrollViewer)GetTemplateChild("InfiniteCanvasScrollViewer");
            var eraseAllButton = (Button)GetTemplateChild("EraseAllButton");

            _canvasTextBox = (InfiniteCanvasTextBox)GetTemplateChild("CanvasTextBox");

            _enableTextButton = (InkToolbarCustomToolButton)GetTemplateChild("EnableTextButton");

            _enableTouchInkingButton = (InkToolbarCustomToggleButton)GetTemplateChild("EnableTouchInkingButton");

            _inkCanvasToolBar = (InkToolbar)GetTemplateChild("canToolBar");

            _inkCanvas = (InkCanvas)GetTemplateChild("inkCanvas");

            _undoButton = (Button)GetTemplateChild("UndoButton");
            _redoButton = (Button)GetTemplateChild("RedoButton");

            _canvasTextBoxFontSizeTextBox.TextChanged += _canvasTextBoxFontSizeTextBox_TextChanged;
            _canvasTextBoxItlaicButton.Click += _canvasTextBoxItlaicButton_Clicked;
            _canvasTextBoxBoldButton.Click += _canvasTextBoxBoldButton_Clicked;

            _canvasTextBoxColorPicker.ColorChanged += _canvasTextBoxColorPicker_ColorChanged;

            _enableTouchInkingButton.Checked += _enableTouchInkingButton_Checked;
            _enableTouchInkingButton.Unchecked += _enableTouchInkingButton_Unchecked;

            _enableTextButton.Checked += _enableTextButton_Checked;
            _enableTextButton.Unchecked += _enableTextButton_Unchecked;
            eraseAllButton.Click += EraseAllButton_Click;

            _infiniteCanvasScrollViewer.PointerPressed += InkScrollViewer_PointerPressed;
            _infiniteCanvasScrollViewer.PreviewKeyDown += InkScrollViewer_PreviewKeyDown;
            _canvasTextBox.TextChanged += _canvasTextBox_TextChanged;
            _canvasTextBox.SizeChanged += _canvasTextBox_SizeChanged;
            _undoButton.Click += _undoButton_Click;
            _redoButton.Click += _redoButton_Click;

            Unloaded += InfiniteCanvas_Unloaded;
            Application.Current.LeavingBackground += Current_LeavingBackground;

            ConfigureControls();
            base.OnApplyTemplate();
        }

        private void ConfigureControls()
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            _inkSync = _inkCanvas.InkPresenter.ActivateCustomDrying();
            _inkCanvas.InkPresenter.StrokesCollected += OnStrokesCollected;
            _inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;

            _infiniteCanvasScrollViewer.MaxZoomFactor = 4.0f;
            _infiniteCanvasScrollViewer.MinZoomFactor = 0.25f;
            _infiniteCanvasScrollViewer.ViewChanged += InkScrollViewer_ViewChanged;
            _infiniteCanvasScrollViewer.SizeChanged += InkScrollViewer_SizeChanged;

            _mainContainer.Width = LargeCanvasWidthHeight;
            _mainContainer.Height = LargeCanvasWidthHeight;
            _inkCanvas.Width = LargeCanvasWidthHeight;
            _inkCanvas.Height = LargeCanvasWidthHeight;
            _drawingSurfaceRenderer.Width = LargeCanvasWidthHeight;
            _drawingSurfaceRenderer.Height = LargeCanvasWidthHeight;

            Canvas.SetLeft(_canvasTextBox, 0);
            Canvas.SetTop(_canvasTextBox, 0);

            _canvasTextBox.UpdateFontSize(TextFontSize);
        }

        private void InfiniteCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.LeavingBackground -= Current_LeavingBackground;
        }

        private void _redoButton_Click(object sender, RoutedEventArgs e)
        {
            _drawingSurfaceRenderer.Redo(ViewPort);
        }

        private void _undoButton_Click(object sender, RoutedEventArgs e)
        {
            _drawingSurfaceRenderer.Undo(ViewPort);
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
            _drawingSurfaceRenderer.ClearAll(ViewPort);
        }

        private async void Current_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            // work around to virtual drawing surface bug.
            await Task.Delay(1000);
            _drawingSurfaceRenderer.ReDraw(ViewPort);
        }

        private void ReDrawCanvas()
        {
            _drawingSurfaceRenderer.ReDraw(ViewPort);
        }

        private void InkScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClearTextBoxValue();
            ReDrawCanvas();
        }

        private void OnStrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            _drawingSurfaceRenderer.ExecuteCreateInk(_inkSync.BeginDry());
            _inkSync.EndDry();
            ReDrawCanvas();
        }

        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (_inkCanvasToolBar.ActiveTool == _inkCanvasToolBar.GetToolButton(InkToolbarTool.Eraser))
            {
                _drawingSurfaceRenderer.Erase(args.CurrentPoint.Position, ViewPort, _infiniteCanvasScrollViewer.ZoomFactor);
            }
        }

        private void InkScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                ClearTextBoxValue();
                ReDrawCanvas();
            }
        }
    }
}
