// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Properties for <see cref="ContentSizer"/>.
    /// </summary>
    public partial class ContentSizer
    {
        /// <summary>
        /// Gets or sets the control that the <see cref="SplitBase"/> is resizing. Be default, this will be the visual ancestor of the <see cref="SplitBase"/>.
        /// </summary>
        public FrameworkElement TargetControl
        {
            get { return (FrameworkElement)GetValue(TargetControlProperty); }
            set { SetValue(TargetControlProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TargetControl"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetControlProperty =
            DependencyProperty.Register(nameof(TargetControl), typeof(FrameworkElement), typeof(SplitBase), new PropertyMetadata(null, OnTargetControlChanged));

        private static void OnTargetControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Should we do this after the TargetControl is Loaded? (And use ActualWidth?)
            // Or should we just do it in the manipulation event if Width is null?

            // Check if our width can be manipulated
            if (d is SplitBase splitterBase && e.NewValue is FrameworkElement element)
            {
                // TODO: For Auto ResizeDirection we might want to do detection logic (TBD) here first?
                if (splitterBase.ResizeDirection != ContentResizeDirection.Horizontal && double.IsNaN(element.Width))
                {
                    // We need to set the Width or Height somewhere,
                    // as if it's NaN we won't be able to manipulate it.
                    element.Width = element.DesiredSize.Width;
                }

                if (splitterBase.ResizeDirection != ContentResizeDirection.Vertical && double.IsNaN(element.Height))
                {
                    element.Height = element.DesiredSize.Height;
                }
            }
        }
    }
}
