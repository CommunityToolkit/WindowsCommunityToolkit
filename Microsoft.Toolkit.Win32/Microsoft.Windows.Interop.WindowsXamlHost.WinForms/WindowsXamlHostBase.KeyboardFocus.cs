// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    using System;
    using MS.Win32;

    /// <summary>
    ///     A sample Windows Forms control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHostBase : System.Windows.Forms.Control
    {
        /// <summary>
        ///     Does this Control currently have focus? Check both the Control's 
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
                    IntPtr focusHandle = UnsafeNativeMethods.GetFocus();
                    return (focusHandle == this.Handle || (xamlIslandWindowHandle != IntPtr.Zero && focusHandle == xamlIslandWindowHandle));
                }

                return false;
            }
        }

        /// <summary>
        ///     Activates the Control
        /// </summary>
        protected override void Select(bool directed, bool forward)
        {
            if (!this.desktopWindowXamlSource.HasFocus)
            {
                this.desktopWindowXamlSource.NavigateFocus(
                    new global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                        global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.First));
            }
             
            base.Select(directed, true);
        }

        /// <summary>
        ///     Processes a command key, ensuring that Xaml has an opportunity 
        ///     to handle the command before normal Windows Forms processing.
        ///     (Xaml must be notified of keys that invoke focus navigation.)
        /// </summary>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (DesignMode)
            {
                return false;
            }

            global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason? xamlSourceFocusNavigationReason;
            switch (keyData)
            {
                // BUGBUG: Bug 18356717: DesktopWindowXamlSource.NavigateFocus non-directional Focus not 
                // moving Focus, not responding to keyboard input. Until then, use Next/Previous only.
                case (System.Windows.Forms.Keys.Tab | System.Windows.Forms.Keys.Shift):
                    xamlSourceFocusNavigationReason = global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Left;
                    break;
                case System.Windows.Forms.Keys.Tab:
                    xamlSourceFocusNavigationReason = global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationReason.Right;
                    break;

                default:
                    xamlSourceFocusNavigationReason = null;
                    break;
            }

            // The key send to this Control instance is not one of the navigation keys handled
            // by global::Windows.UI.Xaml. Allow the base class to handle the key press.
            if (xamlSourceFocusNavigationReason == null)
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            // Determine if the currently focused element is the last element for the requested
            // navigation direction.  If the currently focused element is not the last element 
            // for the requested navigation direction, navigate focus to the next focusable 
            // element.
            global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationResult focusResult;
            if (this.desktopWindowXamlSource.HasFocus)
            {
                focusResult = this.desktopWindowXamlSource.NavigateFocus(
                    new global::Windows.UI.Xaml.Hosting.XamlSourceFocusNavigationRequest(
                        xamlSourceFocusNavigationReason.Value));

                return focusResult.WasFocusMoved;
            }

            return false;
        }
    }
}
