// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// RangeSelector is a "double slider" control for range values.
    /// </summary>
    public partial class RangeSelector : Control
    {
        private double GetPosition(PointerRoutedEventArgs e)
            => e.GetCurrentPoint(_containerCanvas).Position.X;

        private double GetNormlizedPosition(double position)
            => ((position / DragWidth) * (Maximum - Minimum)) + Minimum;

        private void ContainerCanvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", false);
        }

        private void ContainerCanvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_pointerManipulatingMin)
            {
                _pointerManipulatingMin = false;
                _containerCanvas.IsHitTestVisible = true;
                OnValueChanged(new RangeChangedEventArgs(RangeStart, GetNormlizedPosition(GetPosition(e)), RangeSelectorProperty.MinimumValue));
            }
            else if (_pointerManipulatingMax)
            {
                _pointerManipulatingMax = false;
                _containerCanvas.IsHitTestVisible = true;
                OnValueChanged(new RangeChangedEventArgs(RangeEnd, GetNormlizedPosition(GetPosition(e)), RangeSelectorProperty.MaximumValue));
            }

            if (_toolTip != null)
            {
                _toolTip.Visibility = Visibility.Collapsed;
            }

            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void ContainerCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_pointerManipulatingMin)
            {
                _pointerManipulatingMin = false;
                _containerCanvas.IsHitTestVisible = true;
                OnValueChanged(new RangeChangedEventArgs(RangeStart, GetNormlizedPosition(GetPosition(e)), RangeSelectorProperty.MinimumValue));
            }
            else if (_pointerManipulatingMax)
            {
                _pointerManipulatingMax = false;
                _containerCanvas.IsHitTestVisible = true;
                OnValueChanged(new RangeChangedEventArgs(RangeEnd, GetNormlizedPosition(GetPosition(e)), RangeSelectorProperty.MaximumValue));
            }

            SyncThumbs();

            if (_toolTip != null)
            {
                _toolTip.Visibility = Visibility.Collapsed;
            }
        }

        private void ContainerCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = GetPosition(e);
            var normalizedPosition = GetNormlizedPosition(position);

            if (_pointerManipulatingMin && normalizedPosition < RangeEnd)
            {
                RangeStart = DragThumb(_minThumb, 0, Canvas.GetLeft(_maxThumb), position);
                _toolTipText.Text = FormatForToolTip(RangeStart);
            }
            else if (_pointerManipulatingMax && normalizedPosition > RangeStart)
            {
                RangeEnd = DragThumb(_maxThumb, Canvas.GetLeft(_minThumb), DragWidth, position);
                _toolTipText.Text = FormatForToolTip(RangeEnd);
            }
        }

        private void ContainerCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = GetPosition(e);
            var normalizedPosition = position * Math.Abs(Maximum - Minimum) / DragWidth;

            double upperValueDiff = Math.Abs(RangeEnd - normalizedPosition);
            double lowerValueDiff = Math.Abs(RangeStart - normalizedPosition);

            if (upperValueDiff < lowerValueDiff)
            {
                RangeEnd = normalizedPosition;
                _pointerManipulatingMax = true;
                Thumb_DragStarted(_maxThumb);
            }
            else
            {
                RangeStart = normalizedPosition;
                _pointerManipulatingMin = true;
                Thumb_DragStarted(_minThumb);
            }

            SyncThumbs();
        }
    }
}
