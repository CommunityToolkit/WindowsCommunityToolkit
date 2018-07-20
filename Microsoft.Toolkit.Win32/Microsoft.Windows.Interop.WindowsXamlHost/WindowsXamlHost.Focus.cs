// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    partial class WindowsXamlHost : HwndHost
    {
        /// <summary>
        /// Dictionary that maps WPF (host framework) FocusNavigationDirection to UWP XAML XxamlSourceFocusNavigationReason
        /// </summary>
        private static readonly Dictionary<FocusNavigationDirection, Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason>
            MapDirectionToReason =
                new Dictionary<FocusNavigationDirection, Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason>
                {
                    { FocusNavigationDirection.Next,     Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First },
                    { FocusNavigationDirection.First ,   Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First },
                    { FocusNavigationDirection.Previous, Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last },
                    { FocusNavigationDirection.Last,     Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last },
                    { FocusNavigationDirection.Up,       Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Up },
                    { FocusNavigationDirection.Down ,    Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Down },
                    { FocusNavigationDirection.Left,     Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left },
                    { FocusNavigationDirection.Right ,   Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right },
                };

        /// <summary>
        /// Dictionary that maps UWP XAML XamlSourceFocusNavigationReason to WPF (host framework) FocusNavigationDirection
        /// </summary>
        private static readonly Dictionary<Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason, FocusNavigationDirection>
            MapReasonToDirection =
                new Dictionary<Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason, FocusNavigationDirection>()
                {
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First, FocusNavigationDirection.Next },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last,  FocusNavigationDirection.Previous },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Up,    FocusNavigationDirection.Up },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Down,  FocusNavigationDirection.Down },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left,  FocusNavigationDirection.Left },
                    { Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right, FocusNavigationDirection.Right },
                };

        /// <summary>
        /// Last Focus Request GUID to uniquely identify Focus operations, primarily used with error callbacks
        /// </summary>
        private Guid _lastFocusRequest = Guid.Empty;

        /// <summary>
        /// Take Focus Requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTakeFocusRequested(object sender, Windows.UI.Xaml.Hosting.DesktopWindowXamlSourceTakeFocusRequestedEventArgs e)
        {
            if (_lastFocusRequest == e.Request.CorrelationId)
            {
                // If we've arrived at this point, then focus is being move back to us
                // therefore, we should complete the operation to avoid an infinite recursion
                // by "Restoring" the focus back to us under a new correctationId
                var newRequest = new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                    Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Restore);
                _desktopWindowXamlSource.NavigateFocus(newRequest);
            }
            else
            {
                // Last focus request is not initiated by us, so continue
                _lastFocusRequest = e.Request.CorrelationId;
                var direction = MapReasonToDirection[e.Request.Reason];
                var request = new TraversalRequest(direction);
                MoveFocus(request);
            }
        }

        /// <summary>
        /// Transform bounds relative to FrameworkElement
        /// </summary>
        /// <param name="sibling1">TODO</param>
        /// <param name="sibling2">TODO</param>
        /// <returns>TODO</returns>
        private static Windows.Foundation.Rect BoundsRelativeTo(FrameworkElement sibling1, System.Windows.Media.Visual sibling2)
        {
            var origin = default(Windows.Foundation.Rect);

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
        /// Process Tab from host framework
        /// </summary>
        /// <param name="traversalRequest">TraversalRequest that contains requested navigation direction</param>
        /// <returns>Did handle tab</returns>
        protected override bool TabIntoCore(TraversalRequest traversalRequest)
        {
            // Bug 17544829: Focus is wrong if the previous element is in a different FocusScope than the WindowsXamlHost element.
            var focusedElement = FocusManager.GetFocusedElement(
                FocusManager.GetFocusScope(this)) as FrameworkElement;

            var origin = BoundsRelativeTo(focusedElement, this);
            var reason = MapDirectionToReason[traversalRequest.FocusNavigationDirection];
            if (_lastFocusRequest == Guid.Empty)
            {
                _lastFocusRequest = Guid.NewGuid();
            }

            var request = new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(reason, origin, _lastFocusRequest);
            try
            {
                var result = _desktopWindowXamlSource.NavigateFocus(request);

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
        /// TODO: Additional logic for complex Focus scenarios may be required here.  For
        /// now, just call the base class and return.
        /// </summary>
        /// <returns></returns>
        protected override bool HasFocusWithinCore()
        {
            return _desktopWindowXamlSource.HasFocus;
        }

        /// <summary>
        /// Override for OnGotFocus that passes NavigateFocus on to the DesktopXamlSource instance
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (!_desktopWindowXamlSource.HasFocus)
            {
                _desktopWindowXamlSource.NavigateFocus(
                    new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                        Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Programmatic));
            }
        }
    }
}
