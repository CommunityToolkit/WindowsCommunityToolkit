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
    /// Events for <see cref="ContentSizer"/>.
    /// </summary>
    public partial class ContentSizer
    {
        // If no values specified, setup our default behaviors.
        private void ContentSizer_Loaded(object sender, RoutedEventArgs e)
        {
            // Adding Grip to Grid Splitter
            if (Content == null)
            {
                // TODO: Make Converter to put in XAML?
                Content =
                    ResizeDirection == ContentResizeDirection.Vertical ? GripperBarVertical : GripperBarHorizontal;
            }

            if (TargetControl == null)
            {
                TargetControl = this.FindAscendant<FrameworkElement>();
            }
        }

        /// <inheritdoc />
        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            var horizontalChange = e.Delta.Translation.X;
            var verticalChange = e.Delta.Translation.Y;

            if (ResizeDirection == ContentResizeDirection.Vertical)
            {
                if (HorizontalMove(horizontalChange))
                {
                    return;
                }
            }
            else if (ResizeDirection == ContentResizeDirection.Horizontal)
            {
                if (VerticalMove(verticalChange))
                {
                    return;
                }
            }

            base.OnManipulationDelta(e);
        }

        /// <inheritdoc/>
        protected override bool HorizontalMove(double horizontalChange)
        {
            if (TargetControl == null)
            {
                return true;
            }

            horizontalChange = IsDragInverted ? -horizontalChange : horizontalChange;

            if (!IsValidWidth(TargetControl, horizontalChange, ActualWidth))
            {
                return true;
            }

            TargetControl.Width += horizontalChange;

            GripperCursor = CoreCursorType.SizeWestEast;

            return false;
        }

        /// <inheritdoc/>
        protected override bool VerticalMove(double verticalChange)
        {
            if (TargetControl == null)
            {
                return true;
            }

            verticalChange = IsDragInverted ? -verticalChange : verticalChange;

            if (!IsValidHeight(TargetControl, verticalChange, ActualHeight))
            {
                return true;
            }

            // Do we need our ContentResizeDirection to be 4 way? Maybe 'Auto' would check the horizontal/vertical alignment of the target???
            TargetControl.Height += verticalChange;

            GripperCursor = CoreCursorType.SizeNorthSouth;

            return false;
        }
    }
}
