// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    /// InfiniteCanvas is a canvas that supports Ink, Text, Format Text, Zoom in/out, Redo, Undo, Export canvas data, Import canvas data.
    /// </summary>
    public partial class InfiniteCanvas : Control
    {
        private const double DefaultMaxZoomFactor = 4.0;
        private const double DefaultMinZoomFactor = .25;
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
        private FontIcon _fontColorIcon;

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
        /// Gets or sets a value indicating whether the toolbar is visible or not.
        /// </summary>
        public bool IsToolbarVisible
        {
            get { return (bool)GetValue(IsToolbarVisibleProperty); }
            set { SetValue(IsToolbarVisibleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsToolbarVisible "/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsToolbarVisibleProperty =
            DependencyProperty.Register(
                nameof(IsToolbarVisible),
                typeof(bool),
                typeof(InfiniteCanvas),
                new PropertyMetadata(true, IsToolbarVisiblePropertyChanged));

        /// <summary>
        /// Gets or sets the MaxZoomFactor for the canvas, range between 1 to 10 and the default value is 4
        /// </summary>
        public double MaxZoomFactor
        {
            get { return (double)GetValue(MaxZoomFactorProperty); }
            set { SetValue(MaxZoomFactorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MaxZoomFactor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxZoomFactorProperty =
            DependencyProperty.Register(
                nameof(MaxZoomFactor),
                typeof(double),
                typeof(InfiniteCanvas),
                new PropertyMetadata(DefaultMaxZoomFactor, MinMaxZoomChangedPropertyChanged));

        /// <summary>
        /// Gets or sets the MinZoomFactor for the canvas, range between .1 to 1 the default value is .25
        /// </summary>
        public double MinZoomFactor
        {
            get { return (double)GetValue(MinZoomFactorProperty); }
            set { SetValue(MinZoomFactorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="MinZoomFactor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinZoomFactorProperty =
            DependencyProperty.Register(
                nameof(MinZoomFactor),
                typeof(double),
                typeof(InfiniteCanvas),
                new PropertyMetadata(DefaultMinZoomFactor, MinMaxZoomChangedPropertyChanged));

        private Rect ViewPort => new Rect(_infiniteCanvasScrollViewer.HorizontalOffset / _infiniteCanvasScrollViewer.ZoomFactor, _infiniteCanvasScrollViewer.VerticalOffset / _infiniteCanvasScrollViewer.ZoomFactor, ViewPortWidth, ViewPortHeight);

        private double ViewPortHeight => (double.IsNaN(_infiniteCanvasScrollViewer.Height)
            ? Window.Current.Bounds.Height
            : _infiniteCanvasScrollViewer.ViewportHeight) / _infiniteCanvasScrollViewer.ZoomFactor;

        private double ViewPortWidth => (double.IsNaN(_infiniteCanvasScrollViewer.Width)
            ? Window.Current.Bounds.Width
            : _infiniteCanvasScrollViewer.ViewportWidth) / _infiniteCanvasScrollViewer.ZoomFactor;

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
            _fontColorIcon = (FontIcon)GetTemplateChild("FontColorIcon");

            UnRegisterEvents();
            RegisterEvents();

            ConfigureControls();

            if (double.IsNaN(_infiniteCanvasScrollViewer.Width))
            {
                _infiniteCanvasScrollViewer.Width = Window.Current.Bounds.Width;
            }

            if (double.IsNaN(_infiniteCanvasScrollViewer.Height))
            {
                _infiniteCanvasScrollViewer.Height = Window.Current.Bounds.Height;
            }

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
            _canvasTextBoxFontSizeTextBox.PreviewKeyDown -= CanvasTextBoxFontSizeTextBox_PreviewKeyDown;
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
            _canvasTextBoxFontSizeTextBox.PreviewKeyDown += CanvasTextBoxFontSizeTextBox_PreviewKeyDown;
        }

        private void ConfigureControls()
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Pen;
            _inkSync = _inkCanvas.InkPresenter.ActivateCustomDrying();
            _inkCanvas.InkPresenter.StrokesCollected += OnStrokesCollected;

            _inkCanvas.InkPresenter.UnprocessedInput.PointerMoved -= UnprocessedInput_PointerMoved;
            _inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;

            SetZoomFactor();

            _infiniteCanvasScrollViewer.ViewChanged -= InkScrollViewer_ViewChanged;
            _infiniteCanvasScrollViewer.SizeChanged -= InkScrollViewer_SizeChanged;
            _infiniteCanvasScrollViewer.ViewChanged += InkScrollViewer_ViewChanged;
            _infiniteCanvasScrollViewer.SizeChanged += InkScrollViewer_SizeChanged;

            SetCanvasWidthHeight();

            _canvasTextBox.UpdateFontSize(TextFontSize);
        }

        private void SetZoomFactor()
        {
            var maxZoomFactor = DefaultMaxZoomFactor;
            var minZoomFactor = DefaultMinZoomFactor;

            if (MaxZoomFactor >= 1 && MaxZoomFactor <= 10)
            {
                maxZoomFactor = MaxZoomFactor;
            }

            if (MinZoomFactor >= .1 && MinZoomFactor <= 1)
            {
                minZoomFactor = MinZoomFactor;
            }

            _infiniteCanvasScrollViewer.MaxZoomFactor = (float)maxZoomFactor;
            _infiniteCanvasScrollViewer.MinZoomFactor = (float)minZoomFactor;
        }

        private void SetCanvasWidthHeight()
        {
            if (_mainContainer == null || _inkCanvas == null || _drawingSurfaceRenderer == null)
            {
                return;
            }

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
