// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// RangeSelector is a "double slider" control for range values.
    /// </summary>
    public partial class RangeSelector : Control
    {
        /// <summary>
        /// Event raised when lower or upper range thumbs start being dragged.
        /// </summary>
        public event DragStartedEventHandler ThumbDragStarted;

        /// <summary>
        /// Event raised when lower or upper range thumbs end being dragged.
        /// </summary>
        public event DragCompletedEventHandler ThumbDragCompleted;

        private void OnThumbDragStarted(DragStartedEventArgs e)
        {
            ThumbDragStarted?.Invoke(this, e);
        }

        private void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            ThumbDragCompleted?.Invoke(this, e);
        }

        private void MinThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeMin = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), _absolutePosition);

            if (_toolTipText != null)
            {
                _toolTipText.Text = FormatForToolTip(RangeMin);
            }
        }

        private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeMax = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), DragWidth(), _absolutePosition);

            if (_toolTipText != null)
            {
                _toolTipText.Text = FormatForToolTip(RangeMax);
            }
        }

        private void MinThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            OnThumbDragStarted(e);
            Thumb_DragStarted(_minThumb);
        }

        private void MaxThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            OnThumbDragStarted(e);
            Thumb_DragStarted(_maxThumb);
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnThumbDragCompleted(e);
            OnValueChanged(sender.Equals(_minThumb) ? new RangeChangedEventArgs(_oldValue, RangeMin, RangeSelectorProperty.MinimumValue) : new RangeChangedEventArgs(_oldValue, RangeMax, RangeSelectorProperty.MaximumValue));
            SyncThumbs();

            if (_toolTip != null)
            {
                _toolTip.Visibility = Visibility.Collapsed;
            }

            VisualStateManager.GoToState(this, "Normal", true);
        }

        private double DragWidth()
        {
            return _containerCanvas.ActualWidth - _maxThumb.Width;
        }

        private double DragThumb(Thumb thumb, double min, double max, double nextPos)
        {
            nextPos = Math.Max(min, nextPos);
            nextPos = Math.Min(max, nextPos);

            Canvas.SetLeft(thumb, nextPos);

            if (_toolTipText != null && _toolTip != null)
            {
                var thumbCenter = nextPos + (thumb.Width / 2);
                _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var ttWidth = _toolTip.ActualWidth / 2;

                Canvas.SetLeft(_toolTip, thumbCenter - ttWidth);
            }

            return Minimum + ((nextPos / DragWidth()) * (Maximum - Minimum));
        }

        private void Thumb_DragStarted(Thumb thumb)
        {
            var useMin = thumb == _minThumb;
            var otherThumb = useMin ? _maxThumb : _minThumb;

            _absolutePosition = Canvas.GetLeft(thumb);
            Canvas.SetZIndex(thumb, 10);
            Canvas.SetZIndex(otherThumb, 0);
            _oldValue = RangeMin;

            if (_toolTipText != null && _toolTip != null)
            {
                _toolTip.Visibility = Visibility.Visible;
                var thumbCenter = _absolutePosition + (thumb.Width / 2);
                _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var ttWidth = _toolTip.ActualWidth / 2;
                Canvas.SetLeft(_toolTip, thumbCenter - ttWidth);

                _toolTipText.Text = FormatForToolTip(useMin ? RangeMin : RangeMax);
            }

            VisualStateManager.GoToState(this, useMin ? "MinPressed" : "MaxPressed", true);
        }
    }
}
