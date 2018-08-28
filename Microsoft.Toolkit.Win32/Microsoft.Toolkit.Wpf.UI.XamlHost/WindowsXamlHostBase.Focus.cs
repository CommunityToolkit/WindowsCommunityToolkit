// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Toolkit.Wpf.UI.XamlHost
{
    /// <summary>
    /// Focus portion of WindowsXamlHostBase
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        /// Dictionary that maps WPF (host framework) FocusNavigationDirection to UWP XAML XxamlSourceFocusNavigationReason
        /// </summary>
        private static readonly Dictionary<System.Windows.Input.FocusNavigationDirection, Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason>
            MapDirectionToReason =
                new Dictionary<System.Windows.Input.FocusNavigationDirection, Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason>
                {
                    { System.Windows.Input.FocusNavigationDirection.Next,     Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First },
                    { System.Windows.Input.FocusNavigationDirection.First,   Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First },
                    { System.Windows.Input.FocusNavigationDirection.Previous, Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last },
                    { System.Windows.Input.FocusNavigationDirection.Last,     Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last },
                    { System.Windows.Input.FocusNavigationDirection.Up,       Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Up },
                    { System.Windows.Input.FocusNavigationDirection.Down,    Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Down },
                    { System.Windows.Input.FocusNavigationDirection.Left,     Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left },
                    { System.Windows.Input.FocusNavigationDirection.Right,   Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right },
                };

        /// <summary>
        /// Dictionary that maps UWP XAML XamlSourceFocusNavigationReason to WPF (host framework) FocusNavigationDirection
        /// </summary>
        private static readonly Dictionary<Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason, System.Windows.Input.FocusNavigationDirection>
            MapReasonToDirection =
                new Dictionary<Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason, System.Windows.Input.FocusNavigationDirection>()
                {
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First, System.Windows.Input.FocusNavigationDirection.Next },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last,  System.Windows.Input.FocusNavigationDirection.Previous },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Up,    System.Windows.Input.FocusNavigationDirection.Up },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Down,  System.Windows.Input.FocusNavigationDirection.Down },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left,  System.Windows.Input.FocusNavigationDirection.Left },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right, System.Windows.Input.FocusNavigationDirection.Right },
                };

        /// <summary>
        /// Last Focus Request GUID to uniquely identify Focus operations, primarily used with error callbacks
        /// </summary>
        private Guid _lastFocusRequest = Guid.Empty;

        /// <summary>
        /// Override for OnGotFocus that passes NavigateFocus on to the DesktopXamlSource instance
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (!xamlSource.HasFocus)
            {
                xamlSource.NavigateFocus(
                    new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                        Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Programmatic));
            }
        }

        /// <summary>
        /// Process Tab from host framework
        /// </summary>
        /// <param name="request">TraversalRequest that contains requested navigation direction</param>
        /// <returns>Did handle tab</returns>
        protected override bool TabIntoCore(System.Windows.Input.TraversalRequest request)
        {
            // Bug 17544829: Focus is wrong if the previous element is in a different FocusScope than the WindowsXamlHost element.
            var focusedElement = System.Windows.Input.FocusManager.GetFocusedElement(
                System.Windows.Input.FocusManager.GetFocusScope(this)) as FrameworkElement;

            var origin = BoundsRelativeTo(focusedElement, this);
            var reason = MapDirectionToReason[request.FocusNavigationDirection];
            if (_lastFocusRequest == Guid.Empty)
            {
                _lastFocusRequest = Guid.NewGuid();
            }

            var sourceFocusNavigationRequest = new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(reason, origin, _lastFocusRequest);
            try
            {
                var result = xamlSource.NavigateFocus(sourceFocusNavigationRequest);

                // Returning true indicates that focus moved.  This will cause the HwndHost to
                // move focus to the source’s hwnd (call SetFocus Win32 API)
                return result.WasFocusMoved;
            }
            finally
            {
                _lastFocusRequest = Guid.Empty;
            }
        }

        /// <summary>
        /// Transform bounds relative to FrameworkElement
        /// </summary>
        /// <param name="sibling1">base rectangle</param>
        /// <param name="sibling2">second of pair to transform</param>
        /// <returns>result of transformed rectangle</returns>
        private static Windows.Foundation.Rect BoundsRelativeTo(FrameworkElement sibling1, System.Windows.Media.Visual sibling2)
        {
            Windows.Foundation.Rect origin;

            if (sibling1 != null)
            {
                var transform = sibling1.TransformToVisual(sibling2);
                var systemWindowsRect = transform.TransformBounds(
                    new Rect(0, 0, sibling1.ActualWidth, sibling1.ActualHeight));
                origin.X = systemWindowsRect.X;
                origin.Y = systemWindowsRect.Y;
                origin.Width = systemWindowsRect.Width;
                origin.Height = systemWindowsRect.Height;
            }

            return origin;
        }

        /// <summary>
        /// Take Focus Requested
        /// </summary>
        /// <param name="sender">event source</param>
        /// <param name="e">event arguments</param>
        private void OnTakeFocusRequested(object sender, Windows.UI.Xaml.Hosting.DesktopWindowXamlSourceTakeFocusRequestedEventArgs e)
        {
            if (_lastFocusRequest == e.Request.CorrelationId)
            {
                // If we've arrived at this point, then focus is being move back to us
                // therefore, we should complete the operation to avoid an infinite recursion
                // by "Restoring" the focus back to us under a new correctationId
                var newRequest = new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                    Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Restore);
                xamlSource.NavigateFocus(newRequest);
            }
            else
            {
                // Last focus request is not initiated by us, so continue
                _lastFocusRequest = e.Request.CorrelationId;
                var direction = MapReasonToDirection[e.Request.Reason];
                var request = new System.Windows.Input.TraversalRequest(direction);
                MoveFocus(request);
            }
        }
    }
}
