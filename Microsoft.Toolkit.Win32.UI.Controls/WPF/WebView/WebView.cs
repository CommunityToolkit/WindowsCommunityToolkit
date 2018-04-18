// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;
using Point = Windows.Foundation.Point;
using Size = Windows.Foundation.Size;

/*

Overview of Keyboard Input Routing for IWebViewControl

The window hosting IWebViewControl receives regular alphanumeric keyboard input via its WndProc, and whoever is running the message loop
calls DispatchMessage() after any preprocessing and special routing, and the message gets to the WndProc of the window with focus. "Accelerator"
keys need to be passed to the IWebViewControl via TranslateAccelerator()

However, due to the way the window is hosted out of process, we are not able to intercept those messages. As such we have no support to handle
tabbing in or out of the IWebBrowserControl window.

NOTE: Depending on how the control is hosted, input messages flow different

 */

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// This class is an implementation of <see cref="IWebView"/> for WPF. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewHost" />
    /// <seealso cref="Microsoft.Toolkit.Win32.UI.Controls.IWebView" />
    public sealed class WebView : WebViewHost, IWebView
    {
        private static readonly Hashtable InvalidatorMap = new Hashtable();

        private delegate void PropertyInvalidator(WebView webViewHost);

        private static readonly DependencyProperty IsIndexedDBEnabledProperty = DependencyProperty.Register(
            nameof(IsIndexedDBEnabled),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.IsIndexedDBEnabled, PropertyChangedCallback));

        private static readonly DependencyProperty IsJavaScriptEnabledProperty = DependencyProperty.Register(
            nameof(IsJavaScriptEnabled),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.IsJavaScriptEnabled, PropertyChangedCallback));

        private static readonly DependencyProperty IsScriptNotifyAllowedProperty = DependencyProperty.Register(
            nameof(IsScriptNotifyAllowed),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.IsScriptNotifyEnabled, PropertyChangedCallback));

        private static readonly bool IsWebPermissionRestricted = !Security.CallerAndAppDomainHaveUnrestrictedWebBrowserPermission();

        private static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(Uri),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.AboutBlankUri, PropertyChangedCallback));

        private static readonly DependencyProperty IsPrivateNetworkClientServerCapabilityEnabledProperty = DependencyProperty.Register(
            nameof(IsPrivateNetworkClientServerCapabilityEnabled),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.IsPrivateNetworkEnabled, PropertyChangedCallback)
            );

        private WebViewControlProcess _process;
        private WebViewControlHost _webViewControl;

        [SecuritySafeCritical]
        [SuppressMessage("Microsoft.Design", "CA1065")]
        static WebView()
        {
#pragma warning disable 1065
            if (IsWebPermissionRestricted)
            {
                // Could be hosted in non-IE browser (e.g. Firefox) as an Internet-zone XBAP
                // Could also be a standalone ClickOnce application

                // Either way, we don't currently support this

                // TODO: Message
                // ReSharper disable ThrowExceptionInUnexpectedLocation
                throw new InvalidOperationException();

                // ReSharper restore ThrowExceptionInUnexpectedLocation
            }

            // ClickOnce uses AppLaunch.exe to host partial-trust applications
            var hostProcessName = Path.GetFileName(UnsafeNativeMethods.GetModuleFileName(new HandleRef()));
            if (string.Compare(hostProcessName, "AppLaunch.exe", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Not currently supported

                // TODO: Message
                throw new InvalidOperationException();
            }

            // Haven't tested with MTA
            Verify.IsApartmentState(ApartmentState.STA);

            // TODO: Assign Feature Control Keys

            // We use this map to lookup which invalidator method to call when the parent's properties change
            InvalidatorMap[VisibilityProperty] = new PropertyInvalidator(OnVisibilityInvalidated);
            InvalidatorMap[IsEnabledProperty] = new PropertyInvalidator(OnIsEnabledInvalidated);
#pragma warning restore 1065
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var property = e.Property;

            // Look up the property in our map and call the appropriate method to push down
            // the changed value to the hosted control
            if (property != null && InvalidatorMap.ContainsKey(property))
            {
                var invalidator = (PropertyInvalidator)InvalidatorMap[property];
                Verify.IsNotNull(invalidator);
                invalidator(this);
            }
        }

        private static void OnIsEnabledInvalidated(WebView webView)
        {
            Verify.IsNotNull(webView);
            Verify.IsNotNull(webView._webViewControl);
            if (webView?._webViewControl != null)
            {
                // TODO: Is there an equivalent for Win32WebViewHost?
            }
        }

        private static void OnVisibilityInvalidated(WebView webView)
        {
            Verify.IsNotNull(webView);
            Verify.IsNotNull(webView._webViewControl);

            if (webView?._webViewControl != null)
            {
                switch (webView.Visibility)
                {
                    case Visibility.Visible:
                        webView._webViewControl.IsVisible = true;
                        break;
                    case Visibility.Hidden:
                        webView._webViewControl.IsVisible = false;
                        break;
                    case Visibility.Collapsed:
                        webView._webViewControl.IsVisible = false;

                        // TODO: Update bounds to set PreferredSize?
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebView"/> class.
        /// </summary>
        [SecurityCritical]
        public WebView()
        {
            // TODO: Check whether browser is disabled
            // TODO: Handle case (OnLoad) for handling POPUP windows
        }

        /// <summary>
        /// An event that is triggered when the accelerator key is pressed.
        /// </summary>
        public event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed = (sender, args) => { };

        /// <summary>
        /// Occurs when the status of whether the <see cref="WebView"/> current contains a full screen element or not changes.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event does not provide argument deriving from EventArgs")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewContainsFullScreenElement)]
        public event EventHandler<object> ContainsFullScreenElementChanged = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebView"/> has started loading new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewContentLoading)]
        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebView"/> finished parsing the current content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewDomContentLoaded)]

        // ReSharper disable InconsistentNaming
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> has started loading new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameContentLoading)]
        public event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading = (sender, args) => { };

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> finished parsing its current content.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameDomContentLoaded)]

        // ReSharper disable InconsistentNaming
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> FrameDOMContentLoaded = (sender, args) => { };

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> finished navigating to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameNavigationCompleted)]
        public event EventHandler<WebViewControlNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Occurs when a frame in the <see cref="WebView"/> navigates to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameNavigationStarting)]
        public event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting = (sender, args) => { };

        /// <summary>
        /// Occurs periodically while the <see cref="WebView"/> executes JavaScript, letting you halt the script.
        /// </summary>
        /// <remarks>
        /// Your app might appear unresponsive while scripts are running. This event provides an opportunity to interrupt a long-running
        /// script. To determine how long the script has been running, check the <see cref="WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// property of the <see cref="WebViewControlLongRunningScriptDetectedEventArgs"/> object. To halt the script, set the event args
        /// <see cref="WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/> property to true. The halted script will
        /// not execute again unless it is reloaded during a subsequent <see cref="WebView"/> navigation.
        /// </remarks>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs"/>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs.ExecutionTime"/>
        /// <seealso cref="WebViewControlLongRunningScriptDetectedEventArgs.StopPageScriptExecution"/>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewLongRunningScriptDetected)]
        public event EventHandler<WebViewControlLongRunningScriptDetectedEventArgs> LongRunningScriptDetected = (sender, args) => { };

        /// <summary>
        /// Occurs when a focus move is requested.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewMoveFocusRequested)]
        public event EventHandler<WebViewControlMoveFocusRequestedEventArgs> MoveFocusRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when the <see cref="WebView"/> control finished navigating to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNavigationCompleted)]
        public event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

        /// <summary>
        /// Occurs before the <see cref="WebView"/> navigates to new content.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNavigationStarting)]
        public event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting = (sender, args) => { };

        /// <summary>eds
        /// Occurs when an action is performed that causes content to be opened in a new window.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNewWindowRequested)]
        public event EventHandler<WebViewControlNewWindowRequestedEventArgs> NewWindowRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when an action in a <see cref="WebView"/> requires that permission be granted.
        /// </summary>
        /// <remarks>
        /// The types of permission that can be requested are defined in the <see cref="WebViewControlPermissionType"/> enumeration.
        ///
        /// If you don't handle the <see cref="PermissionRequested"/> event, the <see cref="WebView"/> denies permission by default.
        ///
        /// When you handle a permission request in <see cref="WebView"/>, you get a <see cref="WebViewControlPermissionRequest"/> object as
        /// the value of the <see cref="M:WebViewControlPermissionRequestedEventArgs.PermissionRequest"/> property. You can call Allow to grant the request,
        /// Deny to deny the request, or Defer to defer the request until a later time.
        /// </remarks>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewPermissionRequested)]
        public event EventHandler<WebViewControlPermissionRequestedEventArgs> PermissionRequested = (sender, args) => { };

        /// <summary>
        /// Occurs when the content contained in the <see cref="WebView"/> control passes a string to the application by using JavaScript.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewScriptNotify)]
        public event EventHandler<WebViewControlScriptNotifyEventArgs> ScriptNotify = (sender, args) => { };

        /// <summary>
        /// Occurs when <see cref="WebView"/> shows a warning page for content that was reported as unsafe by SmartScreen filter.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnsafeContentWarningDisplaying)]
        public event EventHandler<object> UnsafeContentWarningDisplaying = (sender, args) => { };

        /// <summary>
        /// Occurs when an attempt is made to navigate to a <see cref="Source"/> using a scheme that <see cref="WebView"/> does not support.
        /// </summary>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnsupportedUriSchemeIdentified)]
        public event EventHandler<WebViewControlUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified = (sender, args) => { };

        /// <summary>
        /// Occurs when <see cref="WebView"/> attempts to download an unsupported file.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable")]
        public event EventHandler<WebViewControlUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified = (sender, args) => { };

        /// <summary>
        /// Gets a value indicating whether there is at least one page in the backward navigation history.
        /// </summary>
        /// <value><c>true</c> if the <see cref="WebView"/> can navigate backward; otherwise, <c>false</c>.</value>
        public bool CanGoBack
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.CanGoBack ?? false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there is at least one page in the forward navigation history.
        /// </summary>
        /// <value><c>true</c> if the <see cref="WebView"/> can navigate forward; otherwise, <c>false</c>.</value>
        public bool CanGoForward
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.CanGoForward ?? false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="WebView" /> contains an element that supports full screen.
        /// </summary>
        /// <value><see langword="true" /> if the <see cref="WebView" /> contains an element that supports full screen; otherwise, <see langword="false" />.</value>
        public bool ContainsFullScreenElement
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.ContainsFullScreenElement ?? false;
            }
        }

        /// <summary>
        /// Gets the title of the page currently displayed in the <see cref="WebView" />.
        /// </summary>
        /// <value>The page title.</value>
        public string DocumentTitle
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.DocumentTitle ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the use of IndexedDB is allowed.
        /// </summary>
        /// <value><c>true</c> if IndexedDB is allowed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        /// <see cref="WebViewControlSettings.IsIndexedDBEnabled" />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsIndexedDBEnabled)]
        public bool IsIndexedDBEnabled
        {
            get => (bool)GetValue(IsIndexedDBEnabledProperty);
            set => SetValue(IsIndexedDBEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the use of JavaScript is allowed.
        /// </summary>
        /// <value>true if JavaScript is allowed in the <see cref="WebView" />; otherwise, false. The default is true.</value>
        /// <see cref="WebViewControlSettings.IsJavaScriptEnabled" />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsJavaScriptEnabled)]
        public bool IsJavaScriptEnabled
        {
            get => (bool)GetValue(IsJavaScriptEnabledProperty);
            set => SetValue(IsJavaScriptEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="WebView.ScriptNotify" /> is allowed.
        /// </summary>
        /// <value>Whether <see cref="WebView.ScriptNotify" /> is allowed.</value>
        /// <see cref="WebViewControlSettings.IsScriptNotifyAllowed" />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsScriptNotifyEnabled)]
        public bool IsScriptNotifyAllowed
        {
            get => (bool)GetValue(IsScriptNotifyAllowedProperty);
            set => SetValue(IsScriptNotifyAllowedProperty, value);
        }

        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsPrivateNetworkEnabled)]
        public bool IsPrivateNetworkClientServerCapabilityEnabled
        {
            get => (bool)GetValue(IsPrivateNetworkClientServerCapabilityEnabledProperty);
            set => SetValue(IsPrivateNetworkClientServerCapabilityEnabledProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is web view visible.
        /// </summary>
        /// <value><see langword="true" /> if this instance is web view visible; otherwise, <see langword="false" />.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsWebViewVisible
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl.IsVisible;
            }

            set
            {
                Verify.IsNotNull(_webViewControl);

                if (_webViewControl != null)
                {
                    _webViewControl.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="WebViewControlProcess" /> that the control is hosted in.
        /// </summary>
        /// <value>The <see cref="WebViewControlProcess" /> that the control is hosted in.</value>
        [Browsable(false)]
        public WebViewControlProcess Process
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.Process;
            }
        }

        /// <summary>
        /// Gets a <see cref="WebViewControlSettings" /> object that contains properties to enable or disable <see cref="WebView"/> features.
        /// </summary>
        /// <value>A <see cref="WebViewControlSettings" /> object that contains properties to enable or disable <see cref="WebView"/> features.</value>
        /// <seealso cref="WebViewControlSettings.IsScriptNotifyAllowed" />
        /// <seealso cref="WebViewControlSettings.IsJavaScriptEnabled" />
        /// <seealso cref="WebViewControlSettings.IsIndexedDBEnabled" />
        /// <remarks>Use the <see cref="WebViewControlSettings" /> object to enable or disable the use of JavaScript, ScriptNotify, and IndexedDB in the <see cref="WebView"/>.</remarks>
        [Browsable(false)]
        public WebViewControlSettings Settings
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.Settings;
            }
        }

        /// <summary>
        /// Gets or sets the Uniform Resource Identifier (URI) source of the HTML content to display in the <see cref="WebView"/>.
        /// </summary>
        /// <value>The Uniform Resource Identifier (URI) source of the HTML content to display in the <see cref="WebView"/>.</value>
        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Gets the version of EDGEHTML.DLL used by <see cref="WebView"/>.
        /// </summary>
        /// <value>The version of EDGEHTML.DLL used by <see cref="WebView"/>.</value>
        [Browsable(false)]
        public Version Version
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.Version;
            }
        }

        private bool WebViewControlInitialized => _webViewControl != null;

        /// <summary>
        /// Closes the <see cref="T:Microsoft.Toolkit.Win32.UI.Controls.WPF.WebViewHost" />.
        /// </summary>
        /// <seealso cref="M:System.IDisposable.Dispose" />
        public override void Close()
        {
            // TODO: Guard IsDisposed
            UnsubscribeEvents();
            _webViewControl?.Close();
            _webViewControl?.Dispose();

            _webViewControl = null;
            _process = null;
        }

        /// <summary>
        /// Gets the deferred permission request with the specified <see cref="WebViewControlPermissionRequest.Id" />.
        /// </summary>
        /// <param name="id">The <see cref="WebViewControlPermissionRequest.Id" /> of the deferred permission request.</param>
        /// <returns><see cref="WebViewControlDeferredPermissionRequest" /> The deferred permission request with the specified <see cref="WebViewControlPermissionRequest.Id" />, or null if no permission request with the specified <see cref="WebViewControlPermissionRequest.Id" /> was found.</returns>
        public WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GetDeferredPermissionRequestById(id);
        }

        /// <summary>
        /// Navigates the <see cref="WebView"/> to the previous page in the navigation history.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="WebView"/> navigation to the previous page in the navigation history is successful; otherwise, <c>false</c>.</returns>
        public bool GoBack()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GoBack() ?? false;
        }

        /// <summary>
        /// Navigates the <see cref="WebView"/> to the next page in the navigation history.
        /// </summary>
        /// <returns><c>true</c> if the <see cref="WebView"/> navigation to the next page in the navigation history is successful; otherwise, <c>false</c>.</returns>
        public bool GoForward()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GoForward() ?? false;
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebView"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebView"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName, params string[] arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName, arguments);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebView"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName, IEnumerable<string> arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName, arguments);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebView"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebView"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName, params string[] arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName, arguments);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebView"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName, arguments);
        }

        /// <summary>
        /// Moves the focus.
        /// </summary>
        /// <param name="reason">The reason.</param>
        public void MoveFocus(WebViewControlMoveFocusReason reason)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.MoveFocus(reason);
        }

        /// <exception cref="UriFormatException">
        /// <paramref name="source" /> is empty, or
        /// the scheme specified in <paramref name="source" /> is not correctly formed (see <see cref="M:System.Uri.CheckSchemeName(System.String)" />, or
        /// <paramref name="source" /> contains too many slashes, or
        /// the password specified in <paramref name="source" /> is not valid, or
        /// the host name specified in <paramref name="source" /> is not valid, or
        /// the file name specified in <paramref name="source" /> is not valid, or
        /// the user name specified in <paramref name="source" /> is not valid, or
        /// the host or authority name specified in <paramref name="source" /> cannot be terminated by backslashes, or
        /// the port number specified in <paramref name="source" /> is not valid or cannot be parsed, or
        /// the length of <paramref name="source" /> exceeds 65519 characters, or
        /// the length of the scheme specified in <paramref name="source" /> exceeds 1023 characters, or
        /// there is an invalid character sequence in <paramref name="source" />, or
        /// the MS-DOS path specified in <paramref name="source" /> must start with c:\\.</exception>
        public void Navigate(string source)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Navigate(source);
        }

        /// <summary>
        /// Loads the HTML content at the specified Uniform Resource Identifier (URI).
        /// </summary>
        /// <param name="source">The Uniform Resource Identifier (URI) to load.</param>
        /// <seealso cref="Uri" />
        /// <see cref="Navigate(Uri)" /> is asynchronous. Use the <see cref="NavigationCompleted" /> event to detect when
        /// navigation has completed.
        public void Navigate(Uri source)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);

            // TODO: Support for pack://
            _webViewControl?.Navigate(source);
        }

        /// <summary>
        /// Loads the specified HTML content as a new document.
        /// </summary>
        /// <param name="text">The HTML content to display in the control.</param>
        public void NavigateToString(string text)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.NavigateToString(text);
        }

        /// <summary>
        /// Reloads the current <see cref="Source" /> in the <see cref="WebView"/>.
        /// </summary>
        /// <remarks>If the current source was loaded via <see cref="Navigate(Uri)" />, this method reloads the file without forced cache validation by sending a <c>Pragma:no-cache</c> header to the server.</remarks>
        public void Refresh()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Refresh();
        }

        /// <summary>
        /// Halts the current <see cref="WebView"/> navigation or download.
        /// </summary>
        public void Stop()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Stop();
        }

        protected override void Initialize()
        {
            Verify.IsNull(_process);

            OSVersionHelper.ThrowIfBeforeWindows10RS4();

            var privateNetworkEnabled = !Dispatcher.CheckAccess()
                ? Dispatcher.Invoke(() => IsPrivateNetworkClientServerCapabilityEnabled)
                : IsPrivateNetworkClientServerCapabilityEnabled;

            _process = new WebViewControlProcess(new WebViewControlProcessOptions
            {
                PrivateNetworkClientServerCapability = privateNetworkEnabled
                                                        ? WebViewControlProcessCapabilityState.Enabled
                                                        : WebViewControlProcessCapabilityState.Disabled
            });

            // TODO: Attach WebView to existing process
            Dispatcher.InvokeAsync(async () =>
            {
                Verify.IsNotNull(_process);

                var handle = ChildWindow.Handle;
                var bounds = new Windows.Foundation.Rect(0, 0, RenderSize.Width, RenderSize.Height);

                _webViewControl = await _process.CreateWebViewControlHostAsync(handle, bounds).ConfigureAwait(false);
                Verify.IsNotNull(_webViewControl);

                DestroyWindowCore(ChildWindow);

                SubscribeEvents();
                _webViewControl.IsVisible = true;

                Uri source;
                bool javaScriptEnabled;
                bool indexDBEnabled;
                bool scriptNotifyAllowed;
                if (!Dispatcher.CheckAccess())
                {
                    source = Dispatcher.Invoke(() => Source);
                    javaScriptEnabled = Dispatcher.Invoke(() => IsJavaScriptEnabled);
                    indexDBEnabled = Dispatcher.Invoke(() => IsIndexedDBEnabled);
                    scriptNotifyAllowed = Dispatcher.Invoke(() => IsScriptNotifyAllowed);
                }
                else
                {
                    source = Source;
                    javaScriptEnabled = IsJavaScriptEnabled;
                    indexDBEnabled = IsIndexedDBEnabled;
                    scriptNotifyAllowed = IsScriptNotifyAllowed;
                }

                _webViewControl.Source = source;
                _webViewControl.Settings.IsJavaScriptEnabled = javaScriptEnabled;
                _webViewControl.Settings.IsIndexedDBEnabled = indexDBEnabled;
                _webViewControl.Settings.IsScriptNotifyAllowed = scriptNotifyAllowed;
            });
        }

        private void UpdateBounds(int x, int y, int width, int height, int clientWidth, int clientHeight)
        {
#if DEBUG_LAYOUT
            Debug.WriteLine($"{Name}::{nameof(UpdateBounds)}");
            Debug.Indent();
            Debug.WriteLine($"oldBounds={{x={x} y={y} width={width} height={height} clientWidth={clientWidth} clientHeight={clientHeight}}}");
#endif
            // TODO: Update bounds here to ensure correct draw position?
            // HACK: looks like the vertical pos is counted twice, giving a gap
            y = 0;

#if DEBUG_LAYOUT
            Debug.WriteLine($"newBounds={{x={x} y={y} width={width} height={height} clientWidth={clientWidth} clientHeight={clientHeight}}}");
#endif

            VerifyAccess();
            var rect = new Windows.Foundation.Rect(
                new Point(x, y),
                new Size(width, height));
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.UpdateBounds(rect);

#if DEBUG_LAYOUT
            Debug.Unindent();
#endif

        }

        protected override void UpdateBounds(Rect bounds)
        {
            // TODO: Determine if the coordinates are already transformed for high dpi clients
            var clientWidth = bounds.Width - (bounds.Right - bounds.Left);
            var clientHeight = bounds.Width - (bounds.Bottom - bounds.Top);

            UpdateBounds((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height, (int)clientWidth, (int)clientHeight);
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is WebView wv)
            {
                if (dependencyPropertyChangedEventArgs.Property.Name == nameof(Source))
                {
                    wv.Navigate(dependencyPropertyChangedEventArgs.NewValue as Uri);
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsIndexedDBEnabled))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    wv._webViewControl.Settings.IsIndexedDBEnabled = (bool)dependencyPropertyChangedEventArgs.NewValue;
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsJavaScriptEnabled))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    wv._webViewControl.Settings.IsJavaScriptEnabled = (bool)dependencyPropertyChangedEventArgs.NewValue;
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsScriptNotifyAllowed))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    wv._webViewControl.Settings.IsScriptNotifyAllowed = (bool)dependencyPropertyChangedEventArgs.NewValue;
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsPrivateNetworkClientServerCapabilityEnabled))
                {
                    Verify.IsFalse(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        throw new InvalidOperationException(DesignerUI.InvalidOp_Immutable);
                    }
                }
            }
        }

        private void OnAcceleratorKeyPressed(object sender, WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            var handler = AcceleratorKeyPressed;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContainsFullScreenElementChanged(object sender, object args)
        {
            var handler = ContainsFullScreenElementChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContentLoading(object sender, WebViewControlContentLoadingEventArgs args)
        {
            var handler = ContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnDOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = DOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameContentLoading(object sender, WebViewControlContentLoadingEventArgs args)
        {
            var handler = FrameContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameDOMContentLoaded(object sender, WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = FrameDOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs args)
        {
            var handler = FrameNavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationStarting(object sender, WebViewControlNavigationStartingEventArgs args)
        {
            var handler = FrameNavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnLongRunningScriptDetected(object sender, WebViewControlLongRunningScriptDetectedEventArgs args)
        {
            var handler = LongRunningScriptDetected;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnMoveFocusRequested(object sender, WebViewControlMoveFocusRequestedEventArgs args)
        {
            var handler = MoveFocusRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs args)
        {
            // We could have used
            // if (NavigationCompleted != null) NavigationCompleted(this, args);
            // However, if there is a subscriber and the moment the null check and the call to
            // the event handler by the method is invoked, the subscriber may unsubscribe
            // (e.g. on a different thread) and cause a NullReferenceException.
            // To work around this create a temporally local variable to store the reference and check that
            var handler = NavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationStarting(object sender, WebViewControlNavigationStartingEventArgs args)
        {
            var handler = NavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNewWindowRequested(object sender, WebViewControlNewWindowRequestedEventArgs args)
        {
            var handler = NewWindowRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPermissionRequested(object sender, WebViewControlPermissionRequestedEventArgs args)
        {
            var handler = PermissionRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnScriptNotify(object sender, WebViewControlScriptNotifyEventArgs args)
        {
            var handler = ScriptNotify;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var rect = new Rect(VisualOffset.X, VisualOffset.Y, e.NewSize.Width, e.NewSize.Height);
            UpdateBounds(rect);
        }

        private void OnUnsafeContentWarningDisplaying(object sender, object args)
        {
            var handler = UnsafeContentWarningDisplaying;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsupportedUriSchemeIdentified(object sender, WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            var handler = UnsupportedUriSchemeIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnviewableContentIdentified(object sender, WebViewControlUnviewableContentIdentifiedEventArgs args)
        {
            var handler = UnviewableContentIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void SubscribeEvents()
        {
            SizeChanged += OnSizeChanged;
            Verify.IsNotNull(_webViewControl);
            if (_webViewControl == null)
            {
                return;
            }

            _webViewControl.AcceleratorKeyPressed += OnAcceleratorKeyPressed;
            _webViewControl.ContainsFullScreenElementChanged += OnContainsFullScreenElementChanged;
            _webViewControl.ContentLoading += OnContentLoading;
            _webViewControl.DOMContentLoaded += OnDOMContentLoaded;
            _webViewControl.FrameContentLoading += OnFrameContentLoading;
            _webViewControl.FrameDOMContentLoaded += OnFrameDOMContentLoaded;
            _webViewControl.FrameNavigationCompleted += OnFrameNavigationCompleted;
            _webViewControl.FrameNavigationStarting += OnFrameNavigationStarting;
            _webViewControl.LongRunningScriptDetected += OnLongRunningScriptDetected;
            _webViewControl.MoveFocusRequested += OnMoveFocusRequested;
            _webViewControl.NavigationCompleted += OnNavigationCompleted;
            _webViewControl.NavigationStarting += OnNavigationStarting;
            _webViewControl.NewWindowRequested += OnNewWindowRequested;
            _webViewControl.PermissionRequested += OnPermissionRequested;
            _webViewControl.ScriptNotify += OnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying += OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified += OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified += OnUnviewableContentIdentified;
        }

        private void UnsubscribeEvents()
        {
            SizeChanged -= OnSizeChanged;
            Verify.IsNotNull(_webViewControl);
            if (_webViewControl == null)
            {
                return;
            }

            _webViewControl.AcceleratorKeyPressed -= OnAcceleratorKeyPressed;
            _webViewControl.ContainsFullScreenElementChanged -= OnContainsFullScreenElementChanged;
            _webViewControl.ContentLoading -= OnContentLoading;
            _webViewControl.DOMContentLoaded -= OnDOMContentLoaded;
            _webViewControl.FrameContentLoading -= OnFrameContentLoading;
            _webViewControl.FrameDOMContentLoaded -= OnFrameDOMContentLoaded;
            _webViewControl.FrameNavigationCompleted -= OnFrameNavigationCompleted;
            _webViewControl.FrameNavigationStarting -= OnFrameNavigationStarting;
            _webViewControl.LongRunningScriptDetected -= OnLongRunningScriptDetected;
            _webViewControl.MoveFocusRequested -= OnMoveFocusRequested;
            _webViewControl.NavigationCompleted -= OnNavigationCompleted;
            _webViewControl.NavigationStarting -= OnNavigationStarting;
            _webViewControl.NewWindowRequested -= OnNewWindowRequested;
            _webViewControl.PermissionRequested -= OnPermissionRequested;
            _webViewControl.ScriptNotify -= OnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying -= OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified -= OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified -= OnUnviewableContentIdentified;
        }
    }
}