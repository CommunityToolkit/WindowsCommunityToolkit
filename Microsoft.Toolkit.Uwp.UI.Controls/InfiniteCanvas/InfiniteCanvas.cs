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
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// InfiniteCanvas is an advanced control that supports Ink, Text, Format Text, Zoom in/out, Redo, Undo
    /// </summary>
    public partial class InfiniteCanvas : Control
    {
        private const double LargeCanvasWidthHeight = 1 << 21;
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
        private Button _eraseAllButton;

        private InkToolbar _inkCanvasToolBar;
        private Canvas _mainContainer;
        private ScrollViewer _infiniteCanvasScrollViewer;
        private StackPanel _canvasToolbarContainer;

        /// <summary>
        /// Gets or sets the width of the canvas, default value is the max value 2097152
        /// </summary>
        public double CanvasWidth
        {
            get { return (double)GetValue(CanvasWidthProperty); }
            set { SetValue(CanvasWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CanvasWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanvasWidthProperty =
            DependencyProperty.Register(
                nameof(CanvasWidth),
                typeof(double),
                typeof(InfiniteCanvas),
                new PropertyMetadata(LargeCanvasWidthHeight, CanvasWidthHeightPropertyChanged));

        /// <summary>
        /// Gets or sets the height of the canvas, default value is the max value 2097152
        /// </summary>
        public double CanvasHeight
        {
            get { return (double)GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CanvasHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register(
                nameof(CanvasHeight),
                typeof(double),
                typeof(InfiniteCanvas),
                new PropertyMetadata(LargeCanvasWidthHeight, CanvasWidthHeightPropertyChanged));

        /// <summary>
        /// Gets or sets the visibility of the toolbar.
        /// </summary>
        public Visibility ToolbarVisibility
        {
            get { return (Visibility)GetValue(ToolbarVisibilityProperty); }
            set { SetValue(ToolbarVisibilityProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ToolbarVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToolbarVisibilityProperty =
            DependencyProperty.Register(
                nameof(ToolbarVisibility),
                typeof(Visibility),
                typeof(InfiniteCanvas),
                new PropertyMetadata(Visibility.Visible, ToolbarVisibilityPropertyChanged));

        private Rect ViewPort => new Rect(_infiniteCanvasScrollViewer.HorizontalOffset / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.VerticalOffset / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.ViewportWidth / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.ViewportHeight / _infiniteCanvasScrollViewer.ZoomFactor);

        /// <summary>
        /// Initializes a new instance of the <see cref="InfiniteCanvas"/> class.
        /// </summary>
        public InfiniteCanvas()
        {
            DefaultStyleKey = typeof(InfiniteCanvas);
        }

        /// <inheritdoc />
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
            _eraseAllButton = (Button)GetTemplateChild("EraseAllButton");
            _canvasTextBox = (InfiniteCanvasTextBox)GetTemplateChild("CanvasTextBox");
            _enableTextButton = (InkToolbarCustomToolButton)GetTemplateChild("EnableTextButton");
            _enableTouchInkingButton = (InkToolbarCustomToggleButton)GetTemplateChild("EnableTouchInkingButton");
            _inkCanvasToolBar = (InkToolbar)GetTemplateChild("InkCanvasToolBar");
            _canvasToolbarContainer = (StackPanel)GetTemplateChild("CanvasToolbarContainer");

            _inkCanvas = (InkCanvas)GetTemplateChild("DrawingInkCanvas");
            _undoButton = (Button)GetTemplateChild("UndoButton");
            _redoButton = (Button)GetTemplateChild("RedoButton");

            UnRegisterEvents();
            RegisterEvents();

            ConfigureControls();
            base.OnApplyTemplate();
        }

        private void UnRegisterEvents()
        {
            _canvasTextBoxFontSizeTextBox.TextChanged -= CanvasTextBoxFontSizeTextBox_TextChanged;
            _canvasTextBoxItlaicButton.Click -= CanvasTextBoxItlaicButton_Clicked;
            _canvasTextBoxBoldButton.Click -= CanvasTextBoxBoldButton_Clicked;
            _canvasTextBoxColorPicker.ColorChanged -= CanvasTextBoxColorPicker_ColorChanged;
            _enableTouchInkingButton.Checked -= EnableTouchInkingButton_Checked;
            _enableTouchInkingButton.Unchecked -= EnableTouchInkingButton_Unchecked;
            _enableTextButton.Checked -= EnableTextButton_Checked;
            _enableTextButton.Unchecked -= EnableTextButton_Unchecked;
            _eraseAllButton.Click -= EraseAllButton_Click;
            _infiniteCanvasScrollViewer.PointerPressed -= InkScrollViewer_PointerPressed;
            _infiniteCanvasScrollViewer.PreviewKeyDown -= InkScrollViewer_PreviewKeyDown;
            _canvasTextBox.TextChanged -= CanvasTextBox_TextChanged;
            _canvasTextBox.SizeChanged -= CanvasTextBox_SizeChanged;
            _undoButton.Click -= UndoButton_Click;
            _redoButton.Click -= RedoButton_Click;
            Unloaded -= InfiniteCanvas_Unloaded;
            Application.Current.LeavingBackground -= Current_LeavingBackground;
            _drawingSurfaceRenderer.CommandExecuted -= DrawingSurfaceRenderer_CommandExecuted;
        }

        private void RegisterEvents()
        {
            _canvasTextBoxFontSizeTextBox.TextChanged += CanvasTextBoxFontSizeTextBox_TextChanged;
            _canvasTextBoxItlaicButton.Click += CanvasTextBoxItlaicButton_Clicked;
            _canvasTextBoxBoldButton.Click += CanvasTextBoxBoldButton_Clicked;
            _canvasTextBoxColorPicker.ColorChanged += CanvasTextBoxColorPicker_ColorChanged;
            _enableTouchInkingButton.Checked += EnableTouchInkingButton_Checked;
            _enableTouchInkingButton.Unchecked += EnableTouchInkingButton_Unchecked;
            _enableTextButton.Checked += EnableTextButton_Checked;
            _enableTextButton.Unchecked += EnableTextButton_Unchecked;
            _eraseAllButton.Click += EraseAllButton_Click;
            _infiniteCanvasScrollViewer.PointerPressed += InkScrollViewer_PointerPressed;
            _infiniteCanvasScrollViewer.PreviewKeyDown += InkScrollViewer_PreviewKeyDown;
            _canvasTextBox.TextChanged += CanvasTextBox_TextChanged;
            _canvasTextBox.SizeChanged += CanvasTextBox_SizeChanged;
            _undoButton.Click += UndoButton_Click;
            _redoButton.Click += RedoButton_Click;
            Unloaded += InfiniteCanvas_Unloaded;
            Application.Current.LeavingBackground += Current_LeavingBackground;
            _drawingSurfaceRenderer.CommandExecuted += DrawingSurfaceRenderer_CommandExecuted;
        }

        private void ConfigureControls()
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
            _inkSync = _inkCanvas.InkPresenter.ActivateCustomDrying();
            _inkCanvas.InkPresenter.StrokesCollected += OnStrokesCollected;

            _inkCanvas.InkPresenter.UnprocessedInput.PointerMoved -= UnprocessedInput_PointerMoved;
            _inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;

            _infiniteCanvasScrollViewer.MaxZoomFactor = 4.0f;
            _infiniteCanvasScrollViewer.MinZoomFactor = 0.25f;

            _infiniteCanvasScrollViewer.ViewChanged -= InkScrollViewer_ViewChanged;
            _infiniteCanvasScrollViewer.SizeChanged -= InkScrollViewer_SizeChanged;
            _infiniteCanvasScrollViewer.ViewChanged += InkScrollViewer_ViewChanged;
            _infiniteCanvasScrollViewer.SizeChanged += InkScrollViewer_SizeChanged;

            SetCanvasWidthHeight();

            _canvasTextBox.UpdateFontSize(TextFontSize);
        }

        private void SetCanvasWidthHeight()
        {
            _mainContainer.Width = CanvasWidth;
            _mainContainer.Height = CanvasHeight;
            _inkCanvas.Width = CanvasWidth;
            _inkCanvas.Height = CanvasHeight;
            _drawingSurfaceRenderer.Width = CanvasWidth;
            _drawingSurfaceRenderer.Height = CanvasHeight;
            _drawingSurfaceRenderer.ConfigureSpriteVisual(CanvasWidth, CanvasHeight);
        }

        private void ReDrawCanvas()
        {
            _drawingSurfaceRenderer.ReDraw(ViewPort);
        }

        /// <summary>
        /// Redo the last action.
        /// </summary>
        public void Redo()
        {
            _drawingSurfaceRenderer.Redo(ViewPort);
        }

        /// <summary>
        /// Undo the last action.
        /// </summary>
        public void Undo()
        {
            _drawingSurfaceRenderer.Undo(ViewPort);
        }

        /// <summary>
        /// Export the InfinitCanvas as json string.
        /// </summary>
        /// <returns>json string</returns>
        public string ExportAsJson()
        {
            return _drawingSurfaceRenderer.GetSerializedList();
        }

        /// <summary>
        /// Import InfiniteCanvas from json string and render the new canvas, this function will empty the Redo/Undo queue.
        /// </summary>
        /// <param name="json">InfiniteCanvas json representation</param>
        public void ImportFromJson(string json)
        {
            _drawingSurfaceRenderer.RenderFromJsonAndDraw(ViewPort, json);
        }

        /// <summary>
        /// This event triggered after each render happended because of any change in the canvas elements.
        /// </summary>
        public event EventHandler ReRenderCompleted;
    }
}
