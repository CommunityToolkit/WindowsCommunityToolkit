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
            if (ReadLocalValue(BindingProperty) != DependencyProperty.UnsetValue)
            {
                _currentSize = Binding;
            }
        }

        /// <inheritdoc/>
        protected override bool OnDragHorizontal(double horizontalChange)
        {
            horizontalChange = IsDragInverted ? -horizontalChange : horizontalChange;

            // TODO: Setup a Minimum/Maximum properties to constrain bounds
            ////if (!IsValidWidth(TargetControl, _currentSize + horizontalChange, ActualWidth))
            ////{
            ////    return false;
            ////}

            SetValue(BindingProperty, _currentSize + horizontalChange);

            return true;
        }

        /// <inheritdoc/>
        protected override bool OnDragVertical(double verticalChange)
        {
            verticalChange = IsDragInverted ? -verticalChange : verticalChange;

            ////if (!IsValidHeight(TargetControl, _currentSize + verticalChange, ActualHeight))
            ////{
            ////    return false;
            ////}

            SetValue(BindingProperty, _currentSize + verticalChange);

            return true;
        }
    }
}
