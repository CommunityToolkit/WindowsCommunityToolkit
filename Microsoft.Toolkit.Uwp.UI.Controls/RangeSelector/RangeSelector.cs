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
using System.Linq;
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
    public class RangeSelector : Control
    {
        private const double Epsilon = 0.01;
        private const double DefaultStepFrequency = 1;

        /// <summary>
        /// Identifies the Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, MinimumChangedCallback));

        /// <summary>
        /// Identifies the Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(RangeSelector), new PropertyMetadata(1.0, MaximumChangedCallback));

        /// <summary>
        /// Identifies the RangeMin dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeMinProperty = DependencyProperty.Register(nameof(RangeMin), typeof(double), typeof(RangeSelector), new PropertyMetadata(0.0, RangeMinChangedCallback));

        /// <summary>
        /// Identifies the RangeMax dependency property.
        /// </summary>
        public static readonly DependencyProperty RangeMaxProperty = DependencyProperty.Register(nameof(RangeMax), typeof(double), typeof(RangeSelector), new PropertyMetadata(1.0, RangeMaxChangedCallback));

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
            var normalizedPosition = ((position / _containerCanvas.ActualWidth) * (Maximum - Minimum)) + Minimum;

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
            var normalizedPosition = ((position / _containerCanvas.ActualWidth) * (Maximum - Minimum)) + Minimum;

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
            var normalizedPosition = ((position / _containerCanvas.ActualWidth) * (Maximum - Minimum)) + Minimum;

            if (_pointerManipulatingMin && normalizedPosition < RangeMax)
            {
                RangeMin = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), position);
            }
            else if (_pointerManipulatingMax && normalizedPosition > RangeMin)
            {
                RangeMax = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), _containerCanvas.ActualWidth, position);
            }
        }

        private void ContainerCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_containerCanvas).Position.X;
            var normalizedPosition = position * Math.Abs(Maximum - Minimum) / _containerCanvas.ActualWidth;
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

            if (newValue < oldValue)
            {
                rangeSelector.SyncThumbs();
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

            if (newValue > oldValue)
            {
                rangeSelector.SyncThumbs();
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

        private void SyncThumbs()
        {
            if (_containerCanvas == null)
            {
                return;
            }

            var relativeLeft = ((RangeMin - Minimum) / (Maximum - Minimum)) * _containerCanvas.ActualWidth;
            var relativeRight = ((RangeMax - Minimum) / (Maximum - Minimum)) * _containerCanvas.ActualWidth;

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

        private void MinThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeMin = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), _absolutePosition);
        }

        private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeMax = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), _containerCanvas.ActualWidth, _absolutePosition);
        }

        private double DragThumb(Thumb thumb, double min, double max, double nextPos)
        {
            nextPos = Math.Max(min, nextPos);
            nextPos = Math.Min(max, nextPos);

            Canvas.SetLeft(thumb, nextPos);

            return Minimum + ((nextPos / _containerCanvas.ActualWidth) * (Maximum - Minimum));
        }

        private void MinThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);
            _absolutePosition = Canvas.GetLeft(_minThumb);
            Canvas.SetZIndex(_minThumb, 10);
            Canvas.SetZIndex(_maxThumb, 0);
            _oldValue = RangeMin;

            VisualStateManager.GoToState(this, "MinPressed", true);
        }

        private void MaxThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);
            _absolutePosition = Canvas.GetLeft(_maxThumb);
            Canvas.SetZIndex(_minThumb, 0);
            Canvas.SetZIndex(_maxThumb, 10);
            _oldValue = RangeMax;
            VisualStateManager.GoToState(this, "MaxPressed", true);
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            ThumbDragCompleted?.Invoke(this, e);
            ValueChanged?.Invoke(this, sender.Equals(_minThumb) ? new RangeChangedEventArgs(_oldValue, RangeMin, RangeSelectorProperty.MinimumValue) : new RangeChangedEventArgs(_oldValue, RangeMax, RangeSelectorProperty.MaximumValue));
            SyncThumbs();
            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void RangeSelector_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }
    }
}
