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
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// RangeSelector is a "double slider" control for range values.
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MinPressed", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MaxPressed", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplatePart(Name = "OutOfRangeContentContainer", Type = typeof(Border))]
    [TemplatePart(Name = "ActiveRectangle", Type = typeof(Rectangle))]
    [TemplatePart(Name = "MinThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "MaxThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "ContainerCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "ControlGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "MinValueText", Type = typeof(TextBlock))]
    [TemplatePart(Name = "MaxValueText", Type = typeof(TextBlock))]
    [TemplatePart(Name = "ToolTipText", Type = typeof(TextBlock))]

    public partial class RangeSelector : Control
    {
        private const double Epsilon = 0.01;
        private const double DefaultMinimum = 1;
        private const double DefaultMaximum = 100;
        private const double DefaultStepFrequency = 1;

        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RangeSelector), new PropertyMetadata(DefaultMinimum, MinimumChangedCallback));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RangeSelector), new PropertyMetadata(DefaultMaximum, MaximumChangedCallback));

        /// <summary>
        /// Identifies the RangeMin dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeMinProperty = DependencyProperty.Register(nameof(RangeMin), typeof(double), typeof(RangeSelector), new PropertyMetadata(DefaultMinimum, RangeMinChangedCallback));

        /// <summary>
        /// Identifies the RangeMax dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeMaxProperty = DependencyProperty.Register(nameof(RangeMax), typeof(double), typeof(RangeSelector), new PropertyMetadata(DefaultMaximum, RangeMaxChangedCallback));

        /// <summary>
        /// Identifies the IsTouchOptimized dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTouchOptimizedProperty = DependencyProperty.Register(nameof(IsTouchOptimized), typeof(bool), typeof(RangeSelector), new PropertyMetadata(false, IsTouchOptimizedChangedCallback));

        /// <summary>
        /// Identifies the StepFrequency dependency property.
        /// </summary>
        public static readonly DependencyProperty StepFrequencyProperty = DependencyProperty.Register(nameof(StepFrequency), typeof(double), typeof(RangeSelector), new PropertyMetadata(DefaultStepFrequency));

        private Border _outOfRangeContentContainer;
        private Rectangle _activeRectangle;
        private Thumb _minThumb;
        private Thumb _maxThumb;
        private Canvas _containerCanvas;
        private Grid _controlGrid;
        private double _oldValue;
        private bool _valuesAssigned;
        private bool _minSet;
        private bool _maxSet;
        private bool _pointerManipulatingMin;
        private bool _pointerManipulatingMax;
        private double _absolutePosition;
        private TextBlock _minValueText;
        private TextBlock _maxValueText;
        private TextBlock _toolTipText;

        /// <summary>
        /// Event raised when lower or upper range values are changed.
        /// </summary>
        public event EventHandler<RangeChangedEventArgs> ValueChanged;

        /// <summary>
        /// Event raised when lower or upper range thumbs start being dragged.
        /// </summary>
        public event DragStartedEventHandler ThumbDragStarted;

        /// <summary>
        /// Event raised when lower or upper range thumbs end being dragged.
        /// </summary>
        public event DragCompletedEventHandler ThumbDragCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSelector"/> class.
        /// Create a default range selector control.
        /// </summary>
        public RangeSelector()
        {
            DefaultStyleKey = typeof(RangeSelector);
        }

        /// <summary>
        /// Update the visual state of the control when its template is changed.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (_minThumb != null)
            {
                _minThumb.DragCompleted -= Thumb_DragCompleted;
                _minThumb.DragDelta -= MinThumb_DragDelta;
                _minThumb.DragStarted -= MinThumb_DragStarted;
                _minThumb.KeyDown -= MinThumb_KeyDown;
            }

            if (_maxThumb != null)
            {
                _maxThumb.DragCompleted -= Thumb_DragCompleted;
                _maxThumb.DragDelta -= MaxThumb_DragDelta;
                _maxThumb.DragStarted -= MaxThumb_DragStarted;
                _maxThumb.KeyDown -= MaxThumb_KeyDown;
            }

            if (_containerCanvas != null)
            {
                _containerCanvas.SizeChanged -= ContainerCanvas_SizeChanged;
                _containerCanvas.PointerPressed -= ContainerCanvas_PointerPressed;
                _containerCanvas.PointerMoved -= ContainerCanvas_PointerMoved;
                _containerCanvas.PointerReleased -= ContainerCanvas_PointerReleased;
                _containerCanvas.PointerExited -= ContainerCanvas_PointerExited;
            }

            IsEnabledChanged -= RangeSelector_IsEnabledChanged;

            // Need to make sure the values can be set in XAML and don't overwrite each other
            VerifyValues();
            _valuesAssigned = true;

            _outOfRangeContentContainer = GetTemplateChild("OutOfRangeContentContainer") as Border;
            _activeRectangle = GetTemplateChild("ActiveRectangle") as Rectangle;
            _minThumb = GetTemplateChild("MinThumb") as Thumb;
            _maxThumb = GetTemplateChild("MaxThumb") as Thumb;
            _containerCanvas = GetTemplateChild("ContainerCanvas") as Canvas;
            _controlGrid = GetTemplateChild("ControlGrid") as Grid;
            _minValueText = GetTemplateChild("MinValueText") as TextBlock;
            _maxValueText = GetTemplateChild("MaxValueText") as TextBlock;
            _toolTipText = GetTemplateChild("ToolTipText") as TextBlock;

            if (_minThumb != null)
            {
                _minThumb.DragCompleted += Thumb_DragCompleted;
                _minThumb.DragDelta += MinThumb_DragDelta;
                _minThumb.DragStarted += MinThumb_DragStarted;
                _minThumb.KeyDown += MinThumb_KeyDown;
            }

            if (_maxThumb != null)
            {
                _maxThumb.DragCompleted += Thumb_DragCompleted;
                _maxThumb.DragDelta += MaxThumb_DragDelta;
                _maxThumb.DragStarted += MaxThumb_DragStarted;
                _maxThumb.KeyDown += MaxThumb_KeyDown;
            }

            if (_containerCanvas != null)
            {
                _containerCanvas.SizeChanged += ContainerCanvas_SizeChanged;
                _containerCanvas.PointerEntered += ContainerCanvas_PointerEntered;
                _containerCanvas.PointerPressed += ContainerCanvas_PointerPressed;
                _containerCanvas.PointerMoved += ContainerCanvas_PointerMoved;
                _containerCanvas.PointerReleased += ContainerCanvas_PointerReleased;
                _containerCanvas.PointerExited += ContainerCanvas_PointerExited;
            }

            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", false);

            IsEnabledChanged += RangeSelector_IsEnabledChanged;

            if (IsTouchOptimized)
            {
                ArrangeForTouch();
            }

            UpdateMinimumDisplayText(Minimum, this);
            UpdateMaximumDisplayText(Maximum, this);

            //  Measure our min/max text longest value so we can avoid the length of the scrolling reason shifting in size during use.
            var tb = new TextBlock { Text = Maximum.ToString() };
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            _minValueText.MinWidth = tb.ActualWidth;
            _maxValueText.MinWidth = tb.ActualWidth;

            base.OnApplyTemplate();
        }

        private void MinThumb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Left:
                    RangeMin -= StepFrequency;
                    SyncThumbs();
                    e.Handled = true;
                    break;
                case VirtualKey.Right:
                    RangeMin += StepFrequency;
                    SyncThumbs();
                    e.Handled = true;
                    break;
            }
        }

        private void MaxThumb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Left:
                    RangeMax -= StepFrequency;
                    SyncThumbs();
                    e.Handled = true;
                    break;
                case VirtualKey.Right:
                    RangeMax += StepFrequency;
                    SyncThumbs();
                    e.Handled = true;
                    break;
            }
        }

        private void ContainerCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", false);
        }

        private void ContainerCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_containerCanvas).Position.X;
            var normalizedPosition = ((position / DragWidth()) * (Maximum - Minimum)) + Minimum;

            if (_pointerManipulatingMin)
            {
                _pointerManipulatingMin = false;
                _containerCanvas.IsHitTestVisible = true;
                ValueChanged?.Invoke(this, new RangeChangedEventArgs(RangeMin, normalizedPosition, RangeSelectorProperty.MinimumValue));
            }
            else if (_pointerManipulatingMax)
            {
                _pointerManipulatingMax = false;
                _containerCanvas.IsHitTestVisible = true;
                ValueChanged?.Invoke(this, new RangeChangedEventArgs(RangeMax, normalizedPosition, RangeSelectorProperty.MaximumValue));
            }

            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void ContainerCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_containerCanvas).Position.X;
            var normalizedPosition = ((position / DragWidth()) * (Maximum - Minimum)) + Minimum;

            if (_pointerManipulatingMin)
            {
                _pointerManipulatingMin = false;
                _containerCanvas.IsHitTestVisible = true;
                ValueChanged?.Invoke(this, new RangeChangedEventArgs(RangeMin, normalizedPosition, RangeSelectorProperty.MinimumValue));
            }
            else if (_pointerManipulatingMax)
            {
                _pointerManipulatingMax = false;
                _containerCanvas.IsHitTestVisible = true;
                ValueChanged?.Invoke(this, new RangeChangedEventArgs(RangeMax, normalizedPosition, RangeSelectorProperty.MaximumValue));
            }

            SyncThumbs();
        }

        private void ContainerCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_containerCanvas).Position.X;
            var normalizedPosition = ((position / DragWidth()) * (Maximum - Minimum)) + Minimum;

            if (_pointerManipulatingMin && normalizedPosition < RangeMax)
            {
                RangeMin = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), position);
            }
            else if (_pointerManipulatingMax && normalizedPosition > RangeMin)
            {
                RangeMax = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), DragWidth(), position);
            }
        }

        private void ContainerCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_containerCanvas).Position.X;
            var normalizedPosition = position * Math.Abs(Maximum - Minimum) / DragWidth();
            double upperValueDiff = Math.Abs(RangeMax - normalizedPosition);
            double lowerValueDiff = Math.Abs(RangeMin - normalizedPosition);

            if (upperValueDiff < lowerValueDiff)
            {
                RangeMax = normalizedPosition;
                _pointerManipulatingMax = true;
            }
            else
            {
                RangeMin = normalizedPosition;
                _pointerManipulatingMin = true;
            }

            SyncThumbs();
        }

        private void ContainerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SyncThumbs();
        }

        private void VerifyValues()
        {
            if (Minimum > Maximum)
            {
                Minimum = Maximum;
                Maximum = Maximum;
            }

            if (Minimum == Maximum)
            {
                Maximum += Epsilon;
            }

            if (!_maxSet)
            {
                RangeMax = Maximum;
            }

            if (!_minSet)
            {
                RangeMin = Minimum;
            }

            if (RangeMin < Minimum)
            {
                RangeMin = Minimum;
            }

            if (RangeMax < Minimum)
            {
                RangeMax = Minimum;
            }

            if (RangeMin > Maximum)
            {
                RangeMin = Maximum;
            }

            if (RangeMax > Maximum)
            {
                RangeMax = Maximum;
            }

            if (RangeMax < RangeMin)
            {
                RangeMin = RangeMax;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the range.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }

            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        private static void MinimumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;

            if (rangeSelector == null || !rangeSelector._valuesAssigned)
            {
                return;
            }

            var newValue = (double)e.NewValue;
            var oldValue = (double)e.OldValue;

            if (rangeSelector.Maximum < newValue)
            {
                rangeSelector.Maximum = newValue + Epsilon;
            }

            if (rangeSelector.RangeMin < newValue)
            {
                rangeSelector.RangeMin = newValue;
            }

            if (rangeSelector.RangeMax < newValue)
            {
                rangeSelector.RangeMax = newValue;
            }

            if (newValue != oldValue)
            {
                rangeSelector.SyncThumbs();
                rangeSelector._minValueText.Text = newValue.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the maximum value of the range.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }

            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        private static void MaximumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;

            if (rangeSelector == null || !rangeSelector._valuesAssigned)
            {
                return;
            }

            var newValue = (double)e.NewValue;
            var oldValue = (double)e.OldValue;

            if (rangeSelector.Minimum > newValue)
            {
                rangeSelector.Minimum = newValue - Epsilon;
            }

            if (rangeSelector.RangeMax > newValue)
            {
                rangeSelector.RangeMax = newValue;
            }

            if (rangeSelector.RangeMin > newValue)
            {
                rangeSelector.RangeMin = newValue;
            }

            if (newValue != oldValue)
            {
                rangeSelector.SyncThumbs();
                rangeSelector._maxValueText.Text = newValue.ToString();

            }
        }

        /// <summary>
        /// Gets or sets the current lower limit value of the range.
        /// </summary>
        /// <value>
        /// The current lower limit.
        /// </value>
        public double RangeMin
        {
            get
            {
                return (double)GetValue(RangeMinProperty);
            }

            set
            {
                SetValue(RangeMinProperty, value);
            }
        }

        private static void RangeMinChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;

            if (rangeSelector == null)
            {
                return;
            }

            rangeSelector._minSet = true;

            if (!rangeSelector._valuesAssigned)
            {
                return;
            }

            var newValue = (double)e.NewValue;
            rangeSelector.RangeMinToStepFrequency();

            if (rangeSelector._valuesAssigned)
            {
                UpdateMinimumDisplayText(rangeSelector.RangeMin, rangeSelector);

                if (newValue < rangeSelector.Minimum)
                {
                    rangeSelector.RangeMin = rangeSelector.Minimum;
                    return;
                }

                if (newValue > rangeSelector.Maximum)
                {
                    rangeSelector.RangeMin = rangeSelector.Maximum;
                    return;
                }

                rangeSelector.SyncActiveRectangle();

                // If the new value is greater than the old max, move the max also
                if (newValue > rangeSelector.RangeMax)
                {
                    rangeSelector.RangeMax = newValue;
                }
            }
            else
            {
                rangeSelector.SyncActiveRectangle();
            }
        }

        /// <summary>
        /// Gets or sets the current upper limit value of the range.
        /// </summary>
        /// <value>
        /// The current upper limit.
        /// </value>
        public double RangeMax
        {
            get
            {
                return (double)GetValue(RangeMaxProperty);
            }

            set
            {
                SetValue(RangeMaxProperty, value);
            }
        }

        private static void RangeMaxChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;

            if (rangeSelector == null)
            {
                return;
            }

            rangeSelector._maxSet = true;

            if (!rangeSelector._valuesAssigned)
            {
                return;
            }

            var newValue = (double)e.NewValue;
            rangeSelector.RangeMaxToStepFrequency();

            if (rangeSelector._valuesAssigned)
            {
                UpdateMaximumDisplayText(rangeSelector.RangeMax, rangeSelector);

                if (newValue < rangeSelector.Minimum)
                {
                    rangeSelector.RangeMax = rangeSelector.Minimum;
                    return;
                }

                if (newValue > rangeSelector.Maximum)
                {
                    rangeSelector.RangeMax = rangeSelector.Maximum;
                    return;
                }

                rangeSelector.SyncActiveRectangle();

                // If the new max is less than the old minimum then move the minimum
                if (newValue < rangeSelector.RangeMin)
                {
                    rangeSelector.RangeMin = newValue;
                }
            }
            else
            {
                rangeSelector.SyncActiveRectangle();
            }
        }

        private static void UpdateToolTipText(RangeSelector rangeSelector, TextBlock toolTip, double newValue)
        {
            if (rangeSelector.StepFrequency < 1)
            {
                toolTip.Text = string.Format("{0:0.00}", newValue);
            }
            else
            {
                toolTip.Text = string.Format("{0:0}", newValue);
            }
        }

        private static void UpdateMinimumDisplayText(double newValue, RangeSelector rangeSelector)
        {
            if (rangeSelector.StepFrequency < 1)
            {
                rangeSelector._minValueText.Text = string.Format("{0:0.00}", newValue);
            }
            else
            {
                rangeSelector._minValueText.Text = string.Format("{0:0}", newValue);
            }
        }

        private static void UpdateMaximumDisplayText(double newValue, RangeSelector rangeSelector)
        {
            if (rangeSelector.StepFrequency < 1)
            {
                rangeSelector._maxValueText.Text = string.Format("{0:0.00}", newValue);
            }
            else
            {
                rangeSelector._maxValueText.Text = string.Format("{0:0}", newValue);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is optimized for touch use.
        /// </summary>
        /// <value>
        /// The value for touch optimization.
        /// </value>
        public bool IsTouchOptimized
        {
            get
            {
                return (bool)GetValue(IsTouchOptimizedProperty);
            }

            set
            {
                SetValue(IsTouchOptimizedProperty, value);
            }
        }

        private static void IsTouchOptimizedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rangeSelector = d as RangeSelector;
            if (rangeSelector == null)
            {
                return;
            }

            rangeSelector.ArrangeForTouch();
        }

        /// <summary>
        /// Gets or sets the value part of a value range that steps should be created for.
        /// </summary>
        /// <value>
        /// The value part of a value range that steps should be created for.
        /// </value>
        public double StepFrequency
        {
            get
            {
                return (double)GetValue(StepFrequencyProperty);
            }

            set
            {
                SetValue(StepFrequencyProperty, value);
            }
        }

        private void RangeMinToStepFrequency()
        {
            RangeMin = MoveToStepFrequency(RangeMin);
        }

        private void RangeMaxToStepFrequency()
        {
            RangeMax = MoveToStepFrequency(RangeMax);
        }

        private double MoveToStepFrequency(double rangeValue)
        {
            double newValue = Minimum + (((int)Math.Round((rangeValue - Minimum) / StepFrequency)) * StepFrequency);

            if (newValue < Minimum)
            {
                return Minimum;
            }
            else if (newValue > Maximum || Maximum - newValue < StepFrequency)
            {
                return Maximum;
            }
            else
            {
                return newValue;
            }
        }

        private void ArrangeForTouch()
        {
            if (_containerCanvas == null)
            {
                return;
            }

            if (IsTouchOptimized)
            {
                if (_controlGrid != null)
                {
                    _controlGrid.Height = 44;
                }

                if (_outOfRangeContentContainer != null)
                {
                    _outOfRangeContentContainer.Height = 44;
                }

                if (_minThumb != null)
                {
                    _minThumb.Width = _minThumb.Height = 44;
                    _minThumb.Margin = new Thickness(-20, 0, 0, 0);
                }

                if (_maxThumb != null)
                {
                    _maxThumb.Width = _maxThumb.Height = 44;
                    _maxThumb.Margin = new Thickness(-20, 0, 0, 0);
                }
            }
            else
            {
                if (_controlGrid != null)
                {
                    _controlGrid.Height = 24;
                }

                if (_outOfRangeContentContainer != null)
                {
                    _outOfRangeContentContainer.Height = 24;
                }

                if (_minThumb != null)
                {
                    _minThumb.Width = 8;
                    _minThumb.Height = 24;
                    _minThumb.Margin = new Thickness(-8, 0, 0, 0);
                }

                if (_maxThumb != null)
                {
                    _maxThumb.Width = 8;
                    _maxThumb.Height = 24;
                    _maxThumb.Margin = new Thickness(-8, 0, 0, 0);
                }
            }
        }

        private void SyncThumbs()
        {
            if (_containerCanvas == null)
            {
                return;
            }

            var relativeLeft = ((RangeMin - Minimum) / (Maximum - Minimum)) * DragWidth();
            var relativeRight = ((RangeMax - Minimum) / (Maximum - Minimum)) * DragWidth();

            Canvas.SetLeft(_minThumb, relativeLeft);
            Canvas.SetLeft(_maxThumb, relativeRight);

            SyncActiveRectangle();
        }

        private void SyncActiveRectangle()
        {
            if (_containerCanvas == null)
            {
                return;
            }

            if (_minThumb == null)
            {
                return;
            }

            if (_maxThumb == null)
            {
                return;
            }

            var relativeLeft = Canvas.GetLeft(_minThumb);
            Canvas.SetLeft(_activeRectangle, relativeLeft);
            Canvas.SetTop(_activeRectangle, (_containerCanvas.ActualHeight - _activeRectangle.ActualHeight) / 2);
            _activeRectangle.Width = Math.Max(0, Canvas.GetLeft(_maxThumb) - Canvas.GetLeft(_minThumb));
        }

        private double DragWidth()
        {
            return _containerCanvas.ActualWidth - _maxThumb.Width;
        }

        private void MinThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeMin = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), _absolutePosition);

            UpdateToolTipText(this, _toolTipText, RangeMin);
        }

        private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeMax = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), DragWidth(), _absolutePosition);

            UpdateToolTipText(this, _toolTipText, RangeMax);
        }

        private double DragThumb(Thumb thumb, double min, double max, double nextPos)
        {
            nextPos = Math.Max(min, nextPos);
            nextPos = Math.Min(max, nextPos);

            Canvas.SetLeft(thumb, nextPos);
            var thumbCenter = nextPos + (thumb.Width / 2);
            _toolTipText.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var ttWidth = _toolTipText.ActualWidth / 2;

            Canvas.SetLeft(_toolTipText, thumbCenter - ttWidth);

            return Minimum + ((nextPos / DragWidth()) * (Maximum - Minimum));
        }

        private void MinThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);
            _absolutePosition = Canvas.GetLeft(_minThumb);
            Canvas.SetZIndex(_minThumb, 10);
            Canvas.SetZIndex(_maxThumb, 0);
            _oldValue = RangeMin;
            _toolTipText.Visibility = Visibility.Visible;
            var thumbCenter = _absolutePosition + (_minThumb.Width / 2);
            _toolTipText.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var ttWidth = _toolTipText.ActualWidth / 2;
            Canvas.SetLeft(_toolTipText, thumbCenter - ttWidth);

            VisualStateManager.GoToState(this, "MinPressed", true);
        }

        private void MaxThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);
            _absolutePosition = Canvas.GetLeft(_maxThumb);
            Canvas.SetZIndex(_minThumb, 0);
            Canvas.SetZIndex(_maxThumb, 10);
            _oldValue = RangeMax;
            _toolTipText.Visibility = Visibility.Visible;
            var thumbCenter = _absolutePosition + (_maxThumb.Width / 2);
            _toolTipText.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var ttWidth = _toolTipText.ActualWidth / 2;
            Canvas.SetLeft(_toolTipText, thumbCenter - ttWidth);

            VisualStateManager.GoToState(this, "MaxPressed", true);
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            ThumbDragCompleted?.Invoke(this, e);
            ValueChanged?.Invoke(this, sender.Equals(_minThumb) ? new RangeChangedEventArgs(_oldValue, RangeMin, RangeSelectorProperty.MinimumValue) : new RangeChangedEventArgs(_oldValue, RangeMax, RangeSelectorProperty.MaximumValue));
            SyncThumbs();

            _toolTipText.Visibility = Visibility.Collapsed;

            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void RangeSelector_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }
    }
}
