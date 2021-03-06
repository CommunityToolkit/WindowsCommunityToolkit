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
        private double DragWidth
            => _containerCanvas.ActualWidth - _maxThumb.ActualWidth;

        private void MinThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeStart = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), _absolutePosition);

            if (_toolTipText != null)
            {
                _toolTipText.Text = FormatForToolTip(RangeStart);
            }
        }

        private void MaxThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _absolutePosition += e.HorizontalChange;

            RangeEnd = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), DragWidth, _absolutePosition);

            if (_toolTipText != null)
            {
                _toolTipText.Text = FormatForToolTip(RangeEnd);
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

        private void MinThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnThumbDragCompleted(e);
            OnValueChanged(new RangeChangedEventArgs(_oldValue, RangeStart, RangeSelectorProperty.MinimumValue));
            SyncThumbs();

            if (_toolTip != null)
            {
                _toolTip.Visibility = Visibility.Collapsed;
            }

            VisualStateManager.GoToState(this, "Normal", true);
        }

        private void MaxThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnThumbDragCompleted(e);
            OnValueChanged(new RangeChangedEventArgs(_oldValue, RangeEnd, RangeSelectorProperty.MaximumValue));
            SyncThumbs();

            if (_toolTip != null)
            {
                _toolTip.Visibility = Visibility.Collapsed;
            }

            VisualStateManager.GoToState(this, "Normal", true);
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

            return Minimum + ((nextPos / DragWidth) * (Maximum - Minimum));
        }

        private void Thumb_DragStarted(Thumb thumb)
        {
            var useMin = thumb == _minThumb;
            var otherThumb = useMin ? _maxThumb : _minThumb;

            _absolutePosition = Canvas.GetLeft(thumb);
            Canvas.SetZIndex(thumb, 10);
            Canvas.SetZIndex(otherThumb, 0);
            _oldValue = RangeStart;

            if (_toolTipText != null && _toolTip != null)
            {
                _toolTip.Visibility = Visibility.Visible;
                var thumbCenter = _absolutePosition + (thumb.Width / 2);
                _toolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var ttWidth = _toolTip.ActualWidth / 2;
                Canvas.SetLeft(_toolTip, thumbCenter - ttWidth);

                _toolTipText.Text = FormatForToolTip(useMin ? RangeStart : RangeEnd);
            }

            VisualStateManager.GoToState(this, useMin ? "MinPressed" : "MaxPressed", true);
        }
    }
}
