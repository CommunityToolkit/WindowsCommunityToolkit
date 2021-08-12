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
        //// Value used to determine when we re-calculate in the arrange step or re-use a previous calculation. Within roughly a pixel seems like a good value?
        private const double CalculationTolerance = 1.5;

        private Size _originalSize;
        private Size _lastMeasuredSize;

        private bool IsPositiveRealNumber(double value) => !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            _originalSize = availableSize;

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
            if (Math.Abs(finalSize.Width - _lastMeasuredSize.Width) > CalculationTolerance ||
                Math.Abs(finalSize.Height - _lastMeasuredSize.Height) > CalculationTolerance)
            {
                // Check if we can re-use our measure calculation if we're given effectively
                // the same size as we had in the measure step.
                if (Math.Abs(finalSize.Width - _originalSize.Width) <= CalculationTolerance &&
                    Math.Abs(finalSize.Height - _originalSize.Height) <= CalculationTolerance)
                {
                    finalSize = _lastMeasuredSize;
                }
                else
                {
                    CalculateConstrainedSize(ref finalSize);

                    // Copy again so if Arrange is re-triggered we won't re-re-calculate.
                    _lastMeasuredSize = finalSize;
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        private void CalculateConstrainedSize(ref Size availableSize)
        {
            // 1) We check for Infinity, in the case we have no constraint from parent
            //    we'll request the child's measurements first, so we can use that as
            //    a starting point to constrain it's dimensions based on the criteria
            //    set in our properties.
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

            // 2) Apply Scales to constrain based on a percentage
            // --------------------------------------------------
            availableSize.Width *= ScaleX;
            availableSize.Height *= ScaleY;

            // 3) Apply Multiples
            // ------------------
            // These floor the Width/Height values to the nearest multiple of the property (if set).
            // For instance you may have a responsive 4x4 repeated checkerboard pattern for transparency and
            // want to snap to the nearest interval of 4 so the checkerboard is consistency across the layout.
            if (hasWidth &&
                ReadLocalValue(MultipleXProperty) != DependencyProperty.UnsetValue &&
                MultipleX > 0)
            {
                availableSize.Width -= availableSize.Width % MultipleX;
            }

            if (hasHeight &&
                ReadLocalValue(MultipleYProperty) != DependencyProperty.UnsetValue &&
                MultipleY > 0)
            {
                availableSize.Height -= availableSize.Height % MultipleY;
            }

            // 4) Apply AspectRatio
            // --------------------
            // Finally, we apply the AspectRatio property after we've determined the general
            // area we have to work with based on the other constraints.
            // Devs should be careful if they use both a MultipleX&Y that the AspectRatio is also
            // within that same ratio. The Ratio will take preference here as the last step.
            if (ReadLocalValue(AspectRatioProperty) == DependencyProperty.UnsetValue)
            {
                // Skip as last constraint if we have nothing to do.
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
