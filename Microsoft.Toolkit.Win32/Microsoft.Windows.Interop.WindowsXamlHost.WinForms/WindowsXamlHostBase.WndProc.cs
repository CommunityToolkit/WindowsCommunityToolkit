// <copyright file="WindowsXamlHostBase.WndProc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation
// </copyright>
// <author>Microsoft</author>

using System.Windows.Forms;
using System.Security.Permissions;
using MS.Win32;
using System.ComponentModel;
using System.Drawing;

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    /// <summary>
    ///     A Windows Forms control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHostBase : System.Windows.Forms.Control
    {
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
                Graphics graphics = e.Graphics;

                // Gray background Rectangle
                graphics.FillRectangle(new SolidBrush(Color.DarkGray), ClientRectangle);

                // 'XAML Content' text
                string text1 = "XAML Content";
                using (Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                {
                    Rectangle rect1 = ClientRectangle;

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(text1, font1, Brushes.White, rect1, stringFormat);
                    e.Graphics.DrawRectangle(Pens.Black, rect1);
                }

                return;
            }

            base.OnPaint(e);
        }

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
        /// <param name="m"></param>
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
                    if (UnsafeNativeMethods.IntSetFocus(this.xamlIslandWindowHandle) == System.IntPtr.Zero)
                    {
                        throw new System.InvalidOperationException("WindowsXamlHostBase::WndProc: Failed to SetFocus on UWP XAML window");
                    }
                    base.WndProc(ref m);
                    break;

                case NativeDefines.WM_KILLFOCUS:
                    // If focus is being set on the UWP XAML island window then we should prevent LostFocus by
                    // handling this message.
                    if (this.xamlIslandWindowHandle == null || this.xamlIslandWindowHandle != m.WParam)
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
