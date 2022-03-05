// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Events for <see cref="PropertySizer"/>.
    /// </summary>
    public partial class PropertySizer
    {
        private double _currentSize;

        /// <inheritdoc/>
        protected override void OnDragStarting()
        {
            // We grab the current size of the bound value when we start a drag
            // and we manipulate from that set point.
            if (ReadLocalValue(BindingProperty) != DependencyProperty.UnsetValue)
            {
                _currentSize = Binding;
            }
        }

        /// <inheritdoc/>
        protected override bool OnDragHorizontal(double horizontalChange)
        {
            // We use a central function for both horizontal/vertical as
            // a general property has no notion of direction when we
            // manipulate it, so the logic is abstracted.
            return ApplySizeChange(horizontalChange);
        }

        /// <inheritdoc/>
        protected override bool OnDragVertical(double verticalChange)
        {
            return ApplySizeChange(verticalChange);
        }

        private bool ApplySizeChange(double newSize)
        {
            newSize = IsDragInverted ? -newSize : newSize;

            // We want to be checking the modified final value for bounds checks.
            newSize += _currentSize;

            // Check if we hit the min/max value, as we should use that if we're on the edge
            if (ReadLocalValue(MinimumProperty) != DependencyProperty.UnsetValue &&
                newSize < Minimum)
            {
                // We use SetValue here as that'll update our bound property vs. overwriting the binding itself.
                SetValue(BindingProperty, Minimum);
            }
            else if (ReadLocalValue(MaximumProperty) != DependencyProperty.UnsetValue &&
                newSize > Maximum)
            {
                SetValue(BindingProperty, Maximum);
            }
            else
            {
                // Otherwise, we use the value provided.
                SetValue(BindingProperty, newSize);
            }

            // We're always manipulating the value effectively.
            return true;
        }
    }
}
