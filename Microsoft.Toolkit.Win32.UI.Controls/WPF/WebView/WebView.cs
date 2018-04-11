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

using Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

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
    public sealed class WebView : WebViewHost, IWebView
    {
        private static readonly Hashtable InvalidatorMap = new Hashtable();

        private delegate void PropertyInvalidator(WebView webViewHost);

        private static readonly DependencyProperty IsIndexDBEnabledProperty = DependencyProperty.Register(
            nameof(IsIndexDBEnabled),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(true, PropertyChangedCallback));

        private static readonly DependencyProperty IsJavaScriptEnabledProperty = DependencyProperty.Register(
            nameof(IsJavaScriptEnabled),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(true, PropertyChangedCallback));

        private static readonly DependencyProperty IsScriptNotifyAllowedProperty = DependencyProperty.Register(
            nameof(IsScriptNotifyAllowed),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(true, PropertyChangedCallback));

        private static readonly bool IsWebPermissionRestricted = !Security.CallerAndAppDomainHaveUnrestrictedWebBrowserPermission();

        private static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(Uri),
            typeof(WebView),
            new PropertyMetadata(new Uri("about:blank"), PropertyChangedCallback));

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

            // TODO: OnVisibilityInvalidated

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

        [SecurityCritical]
        public WebView()
        {
            // TODO: Check whether browser is disabled
            // TODO: Handle case (OnLoad) for handling POPUP windows
        }

        public event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed = (sender, args) => { };

        /// <summary>
        /// Occurs when the status of whether the <see cref="WebView"/> current contains a full screen element or not changes.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
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
        public event EventHandler<WebViewNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

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
        /// not execute again unless it is reloaded during a subseqent <see cref="WebView"/> navigation.
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
        public event EventHandler<WebViewNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

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
        /// The types of permission that can be requested are defined in the <see cref="Windows.Web.UI.WebViewControlPermissionType"/> enumeration.
        ///
        /// If you don't handle the <see cref="PermissionRequested"/> event, the <see cref="WebView"/> denies permission by default.
        ///
        /// When you handle a permission request in <see cref="WebView"/>, you get a <see cref="Windows.Web.UI.WebViewControlPermissionRequest"/> object as
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

        public bool CanGoBack
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.CanGoBack ?? false;
            }
        }

        public bool CanGoForward
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.CanGoForward ?? false;
            }
        }

        public bool ContainsFullScreenElement
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.ContainsFullScreenElement ?? false;
            }
        }

        public string DocumentTitle
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.DocumentTitle ?? string.Empty;
            }
        }

        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(false)]
        public bool IsIndexDBEnabled
        {
            get => (bool)GetValue(IsIndexDBEnabledProperty);
            set => SetValue(IsIndexDBEnabledProperty, value);
        }

        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(false)]
        public bool IsJavaScriptEnabled
        {
            get => (bool)GetValue(IsJavaScriptEnabledProperty);
            set => SetValue(IsJavaScriptEnabledProperty, value);
        }

        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(false)]
        public bool IsScriptNotifyAllowed
        {
            get => (bool)GetValue(IsScriptNotifyAllowedProperty);
            set => SetValue(IsScriptNotifyAllowedProperty, value);
        }

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

        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

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

        public override void Close()
        {
            // TODO: Guard IsDisposed

            UnsubscribeEvents();
            _webViewControl?.Close();
            _webViewControl?.Dispose();

            _webViewControl = null;
            _process = null;
        }

        public WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GetDeferredPermissionRequestById(id);
        }

        public bool GoBack()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GoBack() ?? false;
        }

        public bool GoForward()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GoForward() ?? false;
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="Windows.Web.UI.Interop.WebViewControl"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="Windows.Web.UI.Interop.WebViewControl"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName, params string[] arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName, arguments);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="Windows.Web.UI.Interop.WebViewControl"/> is not yet initialized.</exception>
        public object InvokeScript(string scriptName, IEnumerable<string> arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName, arguments);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="Windows.Web.UI.Interop.WebViewControl"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="Windows.Web.UI.Interop.WebViewControl"/> is not yet initialized.</exception>
        public Task<string> InvokeScriptAsync(string scriptName, params string[] arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName, arguments);
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="Windows.Web.UI.Interop.WebViewControl"/> is not yet initialized.</exception>
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
        ///                 In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
        ///               <paramref name="source" /> is empty.-or- The scheme specified in <paramref name="source" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.-or-
        ///               <paramref name="source" /> contains too many slashes.-or- The password specified in <paramref name="source" /> is not valid.-or- The host name specified in <paramref name="source" /> is not valid.-or- The file name specified in <paramref name="source" /> is not valid. -or- The user name specified in <paramref name="source" /> is not valid.-or- The host or authority name specified in <paramref name="source" /> cannot be terminated by backslashes.-or- The port number specified in <paramref name="source" /> is not valid or cannot be parsed.-or- The length of <paramref name="source" /> exceeds 65519 characters.-or- The length of the scheme specified in <paramref name="source" /> exceeds 1023 characters.-or- There is an invalid character sequence in <paramref name="source" />.-or- The MS-DOS path specified in <paramref name="source" /> must start with c:\\.</exception>
        public void Navigate(string source)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Navigate(source);
        }

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

        public void Refresh()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Refresh();
        }

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

            _process = new WebViewControlProcess();
            Verify.IsNotNull(_process);

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
                    indexDBEnabled = Dispatcher.Invoke(() => IsIndexDBEnabled);
                    scriptNotifyAllowed = Dispatcher.Invoke(() => IsScriptNotifyAllowed);
                }
                else
                {
                    source = Source;
                    javaScriptEnabled = IsJavaScriptEnabled;
                    indexDBEnabled = IsIndexDBEnabled;
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
            // HACK: looks like the virtal pos is counted twice, giving a gap
            y = 0;

#if DEBUG_LAYOUT
            Debug.WriteLine($"newBounds={{x={x} y={y} width={width} height={height} clientWidth={clientWidth} clientHeight={clientHeight}}}");
#endif

            VerifyAccess();
            var rect = new Windows.Foundation.Rect(
                new Point(x, y),
                new Size(width, height)
            );
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.UpdateBounds(rect);

#if DEBUG_LAYOUT
            Debug.Unindent();
#endif

        }

        protected override void UpdateBounds(Rect bounds)
        {
            // TODO: Update bounds here to ensure correct draw position?
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
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsIndexDBEnabled))
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

        private void OnFrameNavigationCompleted(object sender, WebViewNavigationCompletedEventArgs args)
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

        private void OnNavigationCompleted(object sender, WebViewNavigationCompletedEventArgs args)

        {
            // We could have used
            // if (NavigationCompleted != null) NavigationCompleted(this, args);
            // However, if there is a subscriber and the moment the null check and the call to
            // the event handler by the method is invoked, the subscriber may unsubscribe
            // (e.g. on a different thread) and cause a NullReferenceException.
            // To work around this create a temporarly local variable to store the reference and check that
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