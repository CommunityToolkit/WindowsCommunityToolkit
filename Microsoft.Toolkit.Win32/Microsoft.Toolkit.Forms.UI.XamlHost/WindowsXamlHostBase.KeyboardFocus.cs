// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Toolkit.Forms.UI.XamlHost.Interop.Win32;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     WindowsXamlHostBase hosts UWP XAML content inside Windows Forms
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        /// Last Focus Request GUID to uniquely identify Focus operations, primarily used with error callbacks
        /// </summary>
        private Guid _lastFocusRequest = Guid.Empty;

        /// <summary>
        ///     Gets a value indicating whether this Control currently has focus. Check both the Control's
        ///     window handle and the hosted Xaml window handle. If either has focus
        ///     then this Control currently has focus.
        /// </summary>
        public override bool Focused
        {
            get
            {
                if (IsHandleCreated)
                {
                    // Get currently focused window handle and compare with Control
                    // and hosted Xaml content window handles
                    var focusHandle = SafeNativeMethods.GetFocus();
                    return focusHandle == Handle || (_xamlIslandWindowHandle != IntPtr.Zero && _xamlSource.HasFocus);
                }

                return false;
            }
        }

        /// <summary>
        ///     Activates the Windows Forms WindowsXamlHost Control
        /// </summary>
        protected override void Select(bool directed, bool forward)
        {
            ProcessTabKey(forward);
        }

        /// <summary>
        ///     Processes a tab key, ensuring that Xaml has an opportunity
        ///     to handle the command before normal Windows Forms processing.
        ///     (Xaml must be notified of keys that invoke focus navigation.)
        /// </summary>
        /// <returns>true if the command was processed</returns>
        protected override bool ProcessTabKey(bool forward)
        {
            // Determine if the currently focused element is the last element for the requested
            // navigation direction.  If the currently focused element is not the last element
            // for the requested navigation direction, navigate focus to the next focusable
            // element.
            if (!_xamlSource.HasFocus)
            {
                if (_lastFocusRequest == Guid.Empty)
                {
                    _lastFocusRequest = Guid.NewGuid();
                }

                var reason = forward ? Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First : Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last;
                var result = _xamlSource.NavigateFocus(new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(reason, default(Windows.Foundation.Rect), _lastFocusRequest));
                if (result.WasFocusMoved)
                {
                    return true;
                }

                return false;
            }
            else
            {
                // Temporary Focus handling for Redstone 5

                // Call Windows.UI.Xaml.Input.FocusManager.TryMoveFocus Next or Previous and return
                Windows.UI.Xaml.Input.FocusNavigationDirection navigationDirection =
                    forward ? Windows.UI.Xaml.Input.FocusNavigationDirection.Next : Windows.UI.Xaml.Input.FocusNavigationDirection.Previous;

                return Windows.UI.Xaml.Input.FocusManager.TryMoveFocus(navigationDirection);
            }
        }

        /// <summary>
        /// Responds to DesktopWindowsXamlSource TakeFocusRequested event
        /// </summary>
        /// <param name="sender">DesktopWindowsXamlSource</param>
        /// <param name="args">DesktopWindowXamlSourceTakeFocusRequestedEventArgs</param>
        private void OnTakeFocusRequested(Windows.UI.Xaml.Hosting.DesktopWindowXamlSource sender, Windows.UI.Xaml.Hosting.DesktopWindowXamlSourceTakeFocusRequestedEventArgs args)
        {
            if (_lastFocusRequest == args.Request.CorrelationId)
            {
                // If we've arrived at this point, then focus is being move back to us
                // therefore, we should complete the operation to avoid an infinite recursion
                // by "Restoring" the focus back to us under a new correctationId
                var newRequest = new Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                    Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Restore);
                _xamlSource.NavigateFocus(newRequest);
            }
            else
            {
                // Focus was not initiated by WindowsXamlHost. Continue processing the Focus request.
                var reason = args.Request.Reason;
                if (reason == Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First || reason == Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Last)
                {
                    var forward = reason == Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First;
                    Parent.SelectNextControl(this, forward, tabStopOnly: true, nested: false, wrap: true);
                }
            }
        }
    }
}