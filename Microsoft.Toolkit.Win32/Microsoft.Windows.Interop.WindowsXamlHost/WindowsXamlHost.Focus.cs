// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Windows.Interop
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    partial class WindowsXamlHost : HwndHost
    {
        #region KeyboardFocus

        /// <summary>
        /// Last Focus Request GUID to uniquely identify Focus operations, primarily used with error callbacks
        /// </summary>
        private Guid lastFocusRequest = Guid.Empty;

        /// <summary>
        /// Dictionary that maps WPF (host framework) FocusNavigationDirection to UWP XAML XxamlSourceFocusNavigationReason
        /// </summary>
        private readonly static Dictionary<System.Windows.Input.FocusNavigationDirection, global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason>
            MapDirectionToReason =
                new Dictionary<System.Windows.Input.FocusNavigationDirection, global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason>
                {
                    { System.Windows.Input.FocusNavigationDirection.Next,     global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First },
                    { System.Windows.Input.FocusNavigationDirection.First ,   global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First },
                    { System.Windows.Input.FocusNavigationDirection.Previous, global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last },
                    { System.Windows.Input.FocusNavigationDirection.Last,     global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last },
                    { System.Windows.Input.FocusNavigationDirection.Up,       global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Up },
                    { System.Windows.Input.FocusNavigationDirection.Down ,    global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Down },
                    { System.Windows.Input.FocusNavigationDirection.Left,     global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left },
                    { System.Windows.Input.FocusNavigationDirection.Right ,   global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right },
                };

        /// <summary>
        /// Dictionary that maps UWP XAML XamlSourceFocusNavigationReason to WPF (host framework) FocusNavigationDirection
        /// </summary>
        private readonly static Dictionary<global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason, System.Windows.Input.FocusNavigationDirection>
            MapReasonToDirection =
                new Dictionary<global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason, System.Windows.Input.FocusNavigationDirection>()
                {
                    { global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First, System.Windows.Input.FocusNavigationDirection.Next },
                    { global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last,  System.Windows.Input.FocusNavigationDirection.Previous },
                    { global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Up,    System.Windows.Input.FocusNavigationDirection.Up },
                    { global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Down,  System.Windows.Input.FocusNavigationDirection.Down },
                    { global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left,  System.Windows.Input.FocusNavigationDirection.Left },
                    { global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right, System.Windows.Input.FocusNavigationDirection.Right },
                };

        /// <summary>
        /// Take Focus Requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTakeFocusRequested(object sender, global::Windows.UI.Xaml.Hosting.DesktopWindowXamlSourceTakeFocusRequestedEventArgs e)
        {
            if (this.lastFocusRequest == e.Request.CorrelationId)
            {
                // If we've arrived at this point, then focus is being move back to us
                // therefore, we should complete the operation to avoid an infinite recursion
                // by "Restoring" the focus back to us under a new correctationId
                global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest newRequest = new global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                    global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Restore);
                this.desktopWindowXamlSource.NavigateFocus(newRequest);
            }
            else
            {
                // Last focus request is not initiated by us, so continue
                this.lastFocusRequest = e.Request.CorrelationId;
                System.Windows.Input.FocusNavigationDirection direction = MapReasonToDirection[e.Request.Reason];
                var request = new System.Windows.Input.TraversalRequest(direction);
                this.MoveFocus(request);
            }
        }

        /// <summary>
        /// Transform bounds relative to FrameworkElement
        /// </summary>
        /// <param name="sibling1"></param>
        /// <param name="sibling2"></param>
        /// <returns></returns>
        private static global::Windows.Foundation.Rect BoundsRelativeTo(System.Windows.FrameworkElement sibling1, System.Windows.Media.Visual sibling2)
        {
            global::Windows.Foundation.Rect origin = new global::Windows.Foundation.Rect();

            if (sibling1 != null)
            {
                System.Windows.Media.GeneralTransform transform = sibling1.TransformToVisual(sibling2);
                System.Windows.Rect systemWindowsRect = transform.TransformBounds(
                    new System.Windows.Rect(0, 0, sibling1.ActualWidth, sibling1.ActualHeight));
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
        /// <param name="request">TraversalRequest that contains requested navigation direction</param>
        /// <returns>Did handle tab</returns>
        protected override bool TabIntoCore(System.Windows.Input.TraversalRequest traversalRequest)
        {
            // Bug 17544829: Focus is wrong if the previous element is in a different FocusScope than the WindowsXamlHost element.
            System.Windows.FrameworkElement focusedElement = System.Windows.Input.FocusManager.GetFocusedElement(
                System.Windows.Input.FocusManager.GetFocusScope(this)) as System.Windows.FrameworkElement;

            global::Windows.Foundation.Rect origin = BoundsRelativeTo(focusedElement, this);
            global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason reason = MapDirectionToReason[traversalRequest.FocusNavigationDirection];
            if (this.lastFocusRequest == System.Guid.Empty)
            {
                this.lastFocusRequest = System.Guid.NewGuid();
            }
            global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest request = new global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(reason, origin, this.lastFocusRequest);
            try
            {
                global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationResult result = this.desktopWindowXamlSource.NavigateFocus(request);

                // Returning true indicates that focus moved.  This will cause the HwndHost to
                // move focus to the source’s hwnd (call SetFocus Win32 API)
                return result.WasFocusMoved;
            }
            finally
            {
                this.lastFocusRequest = System.Guid.Empty;
            }
        } 
        
        /// <summary>
        /// TODO: Additional logic for complex Focus scenarios may be required here.  For
        /// now, just call the base class and return. 
        /// </summary>
        /// <returns></returns>
        protected override bool HasFocusWithinCore()
        {
            return base.HasFocusWithinCore();
        }

        /// <summary>
        /// Override for OnGotFocus that passes NavigateFocus on to the DesktopXamlSource instance
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (!this.desktopWindowXamlSource.HasFocus)
            {
                this.desktopWindowXamlSource.NavigateFocus(
                    new global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                        global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Programmatic));
            }
        }

        #endregion
    }
}
