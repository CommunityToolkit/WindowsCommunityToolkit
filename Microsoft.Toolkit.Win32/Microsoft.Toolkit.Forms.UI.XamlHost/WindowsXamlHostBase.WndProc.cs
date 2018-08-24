// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.Toolkit.Forms.UI.XamlHost.Interop.Win32;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     A Windows Forms control that can be used to host XAML content
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        /// Internal only
        /// </summary>
        /// <internalonly>
        ///     Hide GDI painting because the HwndTarget is going to just bitblt the root
        ///     visual on top of everything.
        /// </internalonly>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnPaint(PaintEventArgs e)
        {
            // Show 'XAML Content' with a gray Rectangle placeholder when running in the Designer
            if (DesignMode)
            {
                var graphics = e.Graphics;

                // Gray background Rectangle
                graphics.FillRectangle(new SolidBrush(Color.DarkGray), ClientRectangle);

                // 'XAML Content' text
                var text1 = "XAML Content";
                using (var font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                {
                    var rect1 = ClientRectangle;

                    var stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(text1, font1, Brushes.White, rect1, stringFormat);
                    e.Graphics.DrawRectangle(Pens.Black, rect1);
                }

                return;
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Internal Only
        /// </summary>
        /// <internalonly>
        ///     Paint our parent's background into an offscreen HBITMAP.
        ///     We then draw this as our background in the hosted Avalon
        ///     control's Render() method to support WinForms->Avalon transparency.
        /// </internalonly>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Do not draw the background
        }

        /// <summary>
        /// Processes Windows messages for XamlContentHost control window (not XAML window)
        /// </summary>
        /// <param name="m">message to process</param>
        [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
            }

            switch (m.Msg)
            {
                // SetDesktopWindowXamlSourceWindowPos must always be called after base.WndProc
                case NativeDefines.WM_MOVE:
                case NativeDefines.WM_SIZE:
                case NativeDefines.WM_WINDOWPOSCHANGED:
                case NativeDefines.WM_WINDOWPOSCHANGING:
                    base.WndProc(ref m);
                    SetDesktopWindowXamlSourceWindowPos();
                    break;

                // BUGBUG: Focus integration with Windows.UI.Xaml.Hosting.XamlSourceFocusNavigation is
                // skipping over nested elements. Update or move back to Windows.Xaml.Input.FocusManager.
                // WM_SETFOCUS should not be handled directly. Bug 18356717: DesktopWindowXamlSource.NavigateFocus
                // non-directional Focus not moving Focus, not responding to keyboard input.
                case NativeDefines.WM_SETFOCUS:
                    if (UnsafeNativeMethods.IntSetFocus(_xamlIslandWindowHandle) == System.IntPtr.Zero)
                    {
                        throw new System.InvalidOperationException("WindowsXamlHostBase::WndProc: Failed to SetFocus on UWP XAML window");
                    }

                    base.WndProc(ref m);
                    break;

                case NativeDefines.WM_KILLFOCUS:
                    // If focus is being set on the UWP XAML island window then we should prevent LostFocus by
                    // handling this message.
                    if (_xamlIslandWindowHandle == null || _xamlIslandWindowHandle != m.WParam)
                    {
                        base.WndProc(ref m);
                    }

                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
