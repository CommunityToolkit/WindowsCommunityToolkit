// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Windows.Web.UI.Interop;
using WebViewControlDeferredPermissionRequest = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlDeferredPermissionRequest;
using WebViewControlMoveFocusReason = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlMoveFocusReason;
using WebViewControlProcess = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlProcess;
using WebViewControlSettings = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlSettings;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// This class is an implementation of <see cref="IWebView" /> for Windows Forms. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="ISupportInitialize" />
    [Designer(typeof(WebViewDesigner))]
    [DefaultProperty(Constants.ComponentDefaultProperty)]
    [DefaultEvent(Constants.ComponentDefaultEvent)]
    [Docking(DockingBehavior.AutoDock)]
    [Description("Embeds a view into your application that renders web content using the Microsoft Edge rendering engine")]
    [SecurityCritical]
    [PermissionSet(SecurityAction.InheritanceDemand, Name = Constants.SecurityPermissionSetName)]
    public sealed partial class WebView : Control, IWebView, ISupportInitialize
    {
        private string _delayedEnterpriseId = WebViewDefaults.EnterpriseId;
        private bool _delayedIsIndexDbEnabled = WebViewDefaults.IsIndexedDBEnabled;
        private bool _delayedIsJavaScriptEnabled = WebViewDefaults.IsJavaScriptEnabled;
        private bool _delayedIsScriptNotifyAllowed = WebViewDefaults.IsScriptNotifyEnabled;
        private bool _delayedPrivateNetworkEnabled = WebViewDefaults.IsPrivateNetworkEnabled;
        private Uri _delayedSource;
        private WebViewControlHost _webViewControl;
        private bool _webViewControlClosed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebView" /> class.
        /// </summary>
        public WebView()
        {
            Paint += OnWebViewPaint;
            Layout += OnWebViewLayout;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="WebView"/> is supported in this environment.
        /// </summary>
        /// <value><see langword="true" /> if this instance is supported; otherwise, <see langword="false" />.</value>
        public static bool IsSupported => WebViewControlHost.IsSupported;

        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ContainsFullScreenElement
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.ContainsFullScreenElement ?? false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="WebView"/> is currently in design mode.
        /// </summary>
        /// <value><see langword="true"/> if the <see cref="WebView"/> is currently in design mode; otherwise, <see langword="false"/>.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool DesignMode => IsInDesignMode();

        /// <summary>
        /// Gets the document title.
        /// </summary>
        /// <value>The document title.</value>
        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DocumentTitle
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.DocumentTitle;
            }
        }

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.EnterpriseId)]
        public string EnterpriseId
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return WebViewControlInitialized
                    ? _webViewControl.Process.EnterpriseId
                    : _delayedEnterpriseId;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                _delayedEnterpriseId = value;
                if (!DesignMode)
                {
                    EnsureInitialized();
                    if (WebViewControlInitialized
                        && !string.Equals(_delayedEnterpriseId, _webViewControl.Process.EnterpriseId, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException(DesignerUI.E_CANNOT_CHANGE_AFTER_INIT);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="WebView" /> is focused.
        /// </summary>
        /// <value><see langword="true" /> if focused; otherwise, <see langword="false" />.</value>
        /// <inheritdoc />
        /// <remarks>Returns <see langword="true" /> if this or any of its child windows has focus.</remarks>
        public override bool Focused
        {
            get
            {
                if (base.Focused)
                {
                    return true;
                }

                var hwndFocus = UnsafeNativeMethods.GetFocus();
                var ret = hwndFocus != IntPtr.Zero
                       && NativeMethods.IsChild(new HandleRef(this, Handle), new HandleRef(null, hwndFocus));

                return ret;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexed database enabled.
        /// </summary>
        /// <value><see langword="true" /> if this instance is indexed database enabled; otherwise, <see langword="false" />.</value>
        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsIndexedDBEnabled)]
        public bool IsIndexedDBEnabled
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return WebViewControlInitialized
                    ? _webViewControl.Settings.IsIndexedDBEnabled
                    : _delayedIsIndexDbEnabled;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                _delayedIsIndexDbEnabled = value;
                if (!DesignMode)
                {
                    EnsureInitialized();
                    if (WebViewControlInitialized)
                    {
                        _webViewControl.Settings.IsIndexedDBEnabled = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the use of JavaScript is allowed.
        /// </summary>
        /// <value><c>true</c> if the use of JavaScript is allowed; otherwise, <c>false</c>.</value>
        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsJavaScriptEnabled)]
        public bool IsJavaScriptEnabled
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return WebViewControlInitialized
                    ? _webViewControl.Settings.IsJavaScriptEnabled
                    : _delayedIsJavaScriptEnabled;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                _delayedIsJavaScriptEnabled = value;
                if (!DesignMode)
                {
                    EnsureInitialized();
                    if (WebViewControlInitialized)
                    {
                        _webViewControl.Settings.IsJavaScriptEnabled = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="E:Microsoft.Toolkit.Win32.UI.Controls.WinForms.WebView.ScriptNotify" /> is allowed;
        /// </summary>
        /// <value><c>true</c> if <see cref="E:Microsoft.Toolkit.Win32.UI.Controls.WinForms.WebView.ScriptNotify" /> is allowed; otherwise, <c>false</c>.</value>
        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsScriptNotifyEnabled)]
        public bool IsScriptNotifyAllowed
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return WebViewControlInitialized
                    ? _webViewControl.Settings.IsScriptNotifyAllowed
                    : _delayedIsScriptNotifyAllowed;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                _delayedIsScriptNotifyAllowed = value;
                if (!DesignMode)
                {
                    EnsureInitialized();
                    if (WebViewControlInitialized)
                    {
                        _webViewControl.Settings.IsScriptNotifyAllowed = value;
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this instance is private network client server capability enabled.
        /// </summary>
        /// <value><see langword="true" /> if this instance is private network client server capability enabled; otherwise, <see langword="false" />.</value>
        /// <exception cref="T:System.InvalidOperationException">Value cannot be set once the control is initialized.</exception>
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsPrivateNetworkEnabled)]
        public bool IsPrivateNetworkClientServerCapabilityEnabled
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return WebViewControlInitialized
                    ? _webViewControl.Process.IsPrivateNetworkClientServerCapabilityEnabled
                    : _delayedPrivateNetworkEnabled;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                _delayedPrivateNetworkEnabled = value;
                if (!DesignMode)
                {
                    EnsureInitialized();
                    if (WebViewControlInitialized
                        && _webViewControl.Process.IsPrivateNetworkClientServerCapabilityEnabled != _delayedPrivateNetworkEnabled)
                    {
                        throw new InvalidOperationException(DesignerUI.E_CANNOT_CHANGE_AFTER_INIT);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="WebViewControlProcess" /> object that the control is hosted in.
        /// </summary>
        /// <value>The <see cref="WebViewControlProcess" /> object that the control is hosted in.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WebViewControlProcess Process { get; private set; }

        /// <summary>
        /// Gets a <see cref="WebViewControlSettings" /> object that contains properties to enable or disable <see cref="WebView" /> features.
        /// </summary>
        /// <value>A <see cref="WebViewControlSettings" /> object that contains properties to enable or disable <see cref="WebView" /> features.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WebViewControlSettings Settings
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return _webViewControl?.Settings;
            }
        }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) source of the HTML content to display in the <see cref="WebView" />.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) source of the HTML content to display in the <see cref="WebView" />.</value>
        [Bindable(true)]
        [StringResourceCategory(Constants.CategoryBehavior)]
        [StringResourceDescription(Constants.DescriptionSource)]
        [TypeConverter(typeof(WebBrowserUriTypeConverter))]
        [DefaultValue((string)null)]
        public Uri Source
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.Implies(Initializing, !Initialized);
                Verify.Implies(Initialized, WebViewControlInitialized);
                return WebViewControlInitialized
                    ? _webViewControl.Source
                    : _delayedSource;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                _delayedSource = value;
                if (!DesignMode)
                {
                    EnsureInitialized();
                    if (WebViewControlInitialized)
                    {
                        if (Initializing && value != null)
                        {
                            // During initialization if there is no Source set a navigation to "about:blank" will occur
                            _webViewControl.Source = value;
                        }
                        else if (Initialized)
                        {
                            // After the control is initialized send all values, regardless of if they are null
                            _webViewControl.Source = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the version of EDGEHTML.DLL used by the control.
        /// </summary>
        /// <value>The version of EDGEHTML.DLL used by the control.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Version Version => _webViewControl?.Version;

        /// <summary>
        /// Closes this control.
        /// </summary>
        public void Close()
        {
            var webViewControlAlreadyClosed = _webViewControlClosed;
            _webViewControlClosed = true;

            // Unsubscribe all events:
            UnsubscribeEvents();

            if (!webViewControlAlreadyClosed)
            {
                _webViewControl?.Close();
                _webViewControl?.Dispose();
            }

            _webViewControl = null;
            Process = null;
        }

        /// <summary>
        /// Gets the deferred permission request with the specified Id.
        /// </summary>
        /// <param name="id">The Id of the deferred permission request.</param>
        /// <returns>A <see cref="WebViewControlDeferredPermissionRequest" /> object of the specified <paramref name="id" />.</returns>
        public WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id) => _webViewControl?.GetDeferredPermissionRequestById(id);

        /// <inheritdoc />
        public string InvokeScript(string scriptName) => _webViewControl?.InvokeScript(scriptName);

        /// <inheritdoc />
        public string InvokeScript(string scriptName, params string[] arguments) => _webViewControl?.InvokeScript(scriptName, arguments);

        /// <inheritdoc />
        public string InvokeScript(string scriptName, IEnumerable<string> arguments) => _webViewControl?.InvokeScript(scriptName, arguments);

        /// <inheritdoc />
        public Task<string> InvokeScriptAsync(string scriptName) => _webViewControl?.InvokeScriptAsync(scriptName);

        /// <inheritdoc />
        public Task<string> InvokeScriptAsync(string scriptName, params string[] arguments) =>
            _webViewControl?.InvokeScriptAsync(scriptName, arguments);

        /// <inheritdoc />
        public Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        => _webViewControl?.InvokeScriptAsync(scriptName, arguments);

        /// <inheritdoc />
        public void MoveFocus(WebViewControlMoveFocusReason reason) => _webViewControl?.MoveFocus(reason);

        /// <inheritdoc />
        public void Navigate(Uri source) => _webViewControl?.Navigate(source);

        /// <inheritdoc />
        public void Navigate(string source)
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Navigate(source);
        }

        /// <inheritdoc />
        public void NavigateToLocal(string relativePath) => _webViewControl?.NavigateToLocal(relativePath);

        /// <inheritdoc />
        public void NavigateToString(string text) => _webViewControl?.NavigateToString(text);

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    Close();
                    _webViewControl?.Dispose();
                    _webViewControl = null;
                    Process = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private bool IsInDesignMode()
        {
            var wpfDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            var formsDesignMode = System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv";
            return wpfDesignMode || formsDesignMode;
        }

        // Ensures the WebViewControl's size stays in sync
        private void OnWebViewLayout(object sender, LayoutEventArgs e)
        {
            // This event is raised once at startup with the AffectedControl and AffectedProperty properties
            // on the LayoutEventArgs as null.
            if (e.AffectedControl != null && e.AffectedProperty != null)
            {
                // Ensure that the affected property is the Bounds property to the control
                if (e.AffectedProperty == nameof(Bounds))
                {
                    // In a typical control the DisplayRectangle is the interior canvas of the control
                    // and in a scrolling control the DisplayRectangle would be larger than the ClientRectangle.
                    // However, that is abstracted from us in WebView so we need to synchronize the ClientRectangle
                    // and permit WebView to handle scrolling based on the new viewport
                    UpdateBounds(ClientRectangle);
                }
            }
        }

        private void OnWebViewPaint(object sender, PaintEventArgs e)
        {
            if (!DesignMode)
            {
                return;
            }

            using (var g = e.Graphics)
            {
                using (var hb = new HatchBrush(HatchStyle.ZigZag, Color.Black, BackColor))
                {
                    g.FillRectangle(hb, ClientRectangle);
                }
            }
        }

        private void UpdateBounds(Rectangle bounds)
        {
            _webViewControl?.UpdateBounds(bounds);
        }
    }
}