// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="ConstrainedBox"/> is a <see cref="FrameworkElement"/> control akin to <see cref="Viewbox"/>
    /// which can modify the behavior of it's child element's layout. <see cref="ConstrainedBox"/> restricts the
    /// available size for its content based on a scale factor, multiple factor, and/or a specific <see cref="AspectRatio"/>, in that order.
    /// This is performed as a layout calculation modification.
    /// </summary>
    /// <remarks>
    /// Note that this class being implemented as a <see cref="ContentPresenter"/> is an implementation detail, and
    /// is not meant to be used as one with a template. It is recommended to avoid styling the frame of the control
    /// with borders and not using <see cref="ContentPresenter.ContentTemplate"/> for future compatibility of your
    /// code if moving to WinUI 3 in the future.
    /// </remarks>
    public partial class ConstrainedBox : ContentPresenter // TODO: Should be FrameworkElement directly, see https://github.com/microsoft/microsoft-ui-xaml/issues/5530
    {
        private bool IsPositiveRealNumber(double value) => !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;

        private Size _lastMeasuredSize;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            CalculateConstrainedSize(ref availableSize);

            _lastMeasuredSize = availableSize;

            // Call base.MeasureOverride so any child elements know what room there is to work with.
            // Don't return this though. An image that hasn't loaded yet for example will request very little space.
            base.MeasureOverride(_lastMeasuredSize);
            return _lastMeasuredSize;
        }

        //// Our Arrange pass should just use the value we calculated in Measure, so we don't have extra work to do (at least the ContentPresenter we use presently does it for us.)

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Even though we requested in measure to be a specific size, that doesn't mean our parent
            // panel respected that request. Grid for instance can by default Stretch and if you don't
            // set Horizontal/VerticalAlignment on the control it won't constrain as we expect.
            // We could also be in a StackPanel/ScrollViewer where it wants to provide as much space as possible.
            // However, if we always re-calculate even if we are provided the proper finalSize, this can trigger
            // multiple arrange passes and cause a rounding error in layout. Therefore, we only want to
            // re-calculate if we think we will have a significant impact.
            //// TODO: Not sure what good tolerance is here
            if (Math.Abs(finalSize.Width - _lastMeasuredSize.Width) > 1.5 ||
                Math.Abs(finalSize.Height - _lastMeasuredSize.Height) > 1.5)
            {
                CalculateConstrainedSize(ref finalSize);

                // Copy again so if Arrange is re-triggered we won't re-calculate.
                _lastMeasuredSize = finalSize;
            }

            return base.ArrangeOverride(finalSize);
        }

        private void CalculateConstrainedSize(ref Size availableSize)
        {
            var hasWidth = IsPositiveRealNumber(availableSize.Width);
            var hasHeight = IsPositiveRealNumber(availableSize.Height);

            if (!hasWidth && !hasHeight)
            {
                // We have infinite space, like a ScrollViewer with both scrolling directions
                // Ask child how big they want to be first.
                availableSize = base.MeasureOverride(availableSize);

                hasWidth = IsPositiveRealNumber(availableSize.Width);
                hasHeight = IsPositiveRealNumber(availableSize.Height);

                if (!hasWidth && !hasHeight)
                {
                    // At this point we have no way to determine a constraint, the Panel won't do anything
                    // This should be rare? We don't really have a way to provide a warning here.
                    return;
                }
            }

            // Scale size first before we constrain aspect ratio
            availableSize.Width *= ScaleX;
            availableSize.Height *= ScaleY;

            // If we don't have an Aspect Ratio, just return the scaled value.
            if (ReadLocalValue(AspectRatioProperty) == DependencyProperty.UnsetValue)
            {
                return;
            }

            // Calculate the Aspect Ratio constraint based on the newly scaled size.
            var currentAspect = availableSize.Width / availableSize.Height;

            if (!hasWidth)
            {
                // If available width is infinite, set width based on height
                availableSize.Width = availableSize.Height * AspectRatio;
            }
            else if (!hasHeight)
            {
                // If avalable height is infinite, set height based on width
                availableSize.Height = availableSize.Width / AspectRatio;
            }
            else if (currentAspect > AspectRatio)
            {
                // If the container aspect ratio is wider than our aspect ratio, set width based on height
                availableSize.Width = availableSize.Height * AspectRatio;
            }
            else
            {
                // If the container aspect ratio is taller than our aspect ratio, set height based on width
                availableSize.Height = availableSize.Width / AspectRatio;
            }
        }
    }
}
