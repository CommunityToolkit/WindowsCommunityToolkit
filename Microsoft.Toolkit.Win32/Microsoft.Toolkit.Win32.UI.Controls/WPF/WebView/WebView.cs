// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using System.Windows.Threading;
using Microsoft.Toolkit.Win32.UI.Controls.Interop;
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
    /// <seealso cref="WebViewHost" />
    /// <seealso cref="IWebView" />
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public sealed class WebView : WebViewHost, IWebView
    {
        private const int InitializationBlockingTime = 200;

        private static readonly DependencyProperty EnterpriseIdProperty = DependencyProperty.Register(
            nameof(EnterpriseId),
            typeof(string),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.EnterpriseId, PropertyChangedCallback));

        private static readonly Hashtable InvalidatorMap = new Hashtable();

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

        private static readonly DependencyProperty IsPrivateNetworkClientServerCapabilityEnabledProperty = DependencyProperty.Register(
            nameof(IsPrivateNetworkClientServerCapabilityEnabled),
            typeof(bool),
            typeof(WebView),
            new PropertyMetadata(WebViewDefaults.IsPrivateNetworkEnabled, PropertyChangedCallback));

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

        private WebViewControlProcess _process;

        private volatile WebViewControlHost _webViewControl;

        // Initialization flag for ISupportInitialize
        private InitializationState _initializationState;

        private ManualResetEvent _initializationComplete;

        [SecuritySafeCritical]
        [SuppressMessage("Microsoft.Design", "CA1065", Justification ="Exceptions thrown to fail as fast as possible.")]
        static WebView()
        {
#pragma warning disable 1065
            if (IsWebPermissionRestricted)
            {
                // Could be hosted in non-IE browser (e.g. Firefox) as an Internet-zone XBAP
                // Could also be a standalone ClickOnce application

                // Either way, we don't currently support this
                throw new NotSupportedException(DesignerUI.E_WEB_PERMISSION_RESTRICTED);
            }

            // ClickOnce uses AppLaunch.exe to host partial-trust applications
#pragma warning disable SA1129 // Do not use default value type constructor
            var hostProcessName = Path.GetFileName(UnsafeNativeMethods.GetModuleFileName(new HandleRef()));
#pragma warning restore SA1129 // Do not use default value type constructor
            if (string.Compare(hostProcessName, "AppLaunch.exe", StringComparison.OrdinalIgnoreCase) == 0)
            {
                // Not currently supported
                throw new NotSupportedException(DesignerUI.E_CLICKONCE_PARTIAL_TRUST);
            }

            // Haven't tested with MTA
            Verify.IsApartmentState(ApartmentState.STA);

            // TODO: Assign Feature Control Keys

            // We use this map to lookup which invalidator method to call when the parent's properties change
            InvalidatorMap[VisibilityProperty] = new PropertyInvalidator(OnVisibilityInvalidated);
            InvalidatorMap[IsEnabledProperty] = new PropertyInvalidator(OnIsEnabledInvalidated);
#pragma warning restore 1065
        }

        private bool WebViewInitialized => _initializationState == InitializationState.IsInitialized;

        private bool WebViewInitializing => _initializationState == InitializationState.IsInitializing;

        private bool WebViewControlInitialized => _webViewControl != null && WebViewInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebView"/> class.
        /// </summary>
        [SecurityCritical]
        public WebView()
        {
            // TODO: Check whether browser is disabled
            // TODO: Handle case (OnLoad) for handling POPUP windows
            _initializationComplete = new ManualResetEvent(false);
        }

        internal WebView(WebViewControlHost webViewControl)
            : this()
        {
            _webViewControl = webViewControl ?? throw new ArgumentNullException(nameof(webViewControl));
            _process = webViewControl.Process;
        }

        /// <summary>
        /// Starts the initialization process for this element.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// May occur if the element is already initialized or is already initializing.
        /// </exception>
        /// <inheritdoc cref="FrameworkElement.BeginInit" />
        public override void BeginInit()
        {
            if (WebViewInitialized)
            {
                // Cannot initialize WebView since it is already completely initialized
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_ALREADY_INITIALIZED);
            }

            if (WebViewInitializing)
            {
                // Cannot initialize WebView since it is already being initialized
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_ALREADY_INITIALIZING);
            }

            _initializationState = InitializationState.IsInitializing;

            base.BeginInit();
        }

        /// <summary>
        /// Indicates that the initialization process for the element is complete.
        /// </summary>
        /// <exception cref="InvalidOperationException">May occur when <see cref="BeginInit"/> is not previously called.</exception>
        /// <inheritdoc cref="FrameworkElement.EndInit" />
        public override void EndInit()
        {
            if (!WebViewInitializing)
            {
                // Cannot complete WebView initialization that is not being initialized
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_NOT_INITIALIZING);
            }

            base.EndInit();
        }

        // Ensures this class is initialized. Initialization involves using ISupportInitialize methods
        private void EnsureInitialized()
        {
            // If not already initialized and not already initializing
            if (!WebViewInitialized && !WebViewInitializing)
            {
                BeginInit();
                EndInit();
            }
        }

        private delegate void PropertyInvalidator(WebView webViewHost);

        /// <inheritdoc />
        public event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed = (sender, args) => { };

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event does not provide argument deriving from EventArgs")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewContainsFullScreenElement)]
        public event EventHandler<object> ContainsFullScreenElementChanged = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewContentLoading)]
        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewDomContentLoaded)]
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameContentLoading)]
        public event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading = (sender, args) => { };

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM", Justification = "Name is the same as the WinRT type")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameDomContentLoaded)]
        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> FrameDOMContentLoaded = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameNavigationCompleted)]
        public event EventHandler<WebViewControlNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewFrameNavigationStarting)]
        public event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewLongRunningScriptDetected)]
        public event EventHandler<WebViewControlLongRunningScriptDetectedEventArgs> LongRunningScriptDetected = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewMoveFocusRequested)]
        public event EventHandler<WebViewControlMoveFocusRequestedEventArgs> MoveFocusRequested = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNavigationCompleted)]
        public event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

        /// <inheritdoc/>
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNavigationStarting)]
        public event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewNewWindowRequested)]
        public event EventHandler<WebViewControlNewWindowRequestedEventArgs> NewWindowRequested = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewPermissionRequested)]
        public event EventHandler<WebViewControlPermissionRequestedEventArgs> PermissionRequested = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewScriptNotify)]
        public event EventHandler<WebViewControlScriptNotifyEventArgs> ScriptNotify = (sender, args) => { };

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "WinRT type signature does not have argument that inherits from EventArgs")]
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnsafeContentWarningDisplaying)]
        public event EventHandler<object> UnsafeContentWarningDisplaying = (sender, args) => { };

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryAction)]
        [StringResourceDescription(Constants.DescriptionWebViewUnsupportedUriSchemeIdentified)]
        public event EventHandler<WebViewControlUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified = (sender, args) => { };

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable", Justification = "Unviewable is in WinRT type")]
        public event EventHandler<WebViewControlUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified = (sender, args) => { };

        /// <summary>
        /// Gets a value indicating whether <see cref="WebView"/> is supported in this environment.
        /// </summary>
        /// <value><see langword="true" /> if this instance is supported; otherwise, <see langword="false" />.</value>
        public static bool IsSupported => WebViewControlHost.IsSupported;

        /// <inheritdoc />
        public bool CanGoBack
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.CanGoBack ?? false;
            }
        }

        /// <inheritdoc />
        public bool CanGoForward
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.CanGoForward ?? false;
            }
        }

        /// <inheritdoc />
        public bool ContainsFullScreenElement
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.ContainsFullScreenElement ?? false;
            }
        }

        /// <inheritdoc />
        public string DocumentTitle
        {
            get
            {
                VerifyAccess();
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.DocumentTitle ?? string.Empty;
            }
        }

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.EnterpriseId)]
        public string EnterpriseId
        {
            get => (string)GetValue(EnterpriseIdProperty);
            set => SetValue(EnterpriseIdProperty, value);
        }

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsIndexedDBEnabled)]
        public bool IsIndexedDBEnabled
        {
            get => (bool)GetValue(IsIndexedDBEnabledProperty);
            set => SetValue(IsIndexedDBEnabledProperty, value);
        }

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsJavaScriptEnabled)]
        public bool IsJavaScriptEnabled
        {
            get => (bool)GetValue(IsJavaScriptEnabledProperty);
            set => SetValue(IsJavaScriptEnabledProperty, value);
        }

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsPrivateNetworkEnabled)]
        public bool IsPrivateNetworkClientServerCapabilityEnabled
        {
            get => (bool)GetValue(IsPrivateNetworkClientServerCapabilityEnabledProperty);
            set => SetValue(IsPrivateNetworkClientServerCapabilityEnabledProperty, value);
        }

        /// <inheritdoc />
        [StringResourceCategory(Constants.CategoryBehavior)]
        [DefaultValue(WebViewDefaults.IsScriptNotifyEnabled)]
        public bool IsScriptNotifyAllowed
        {
            get => (bool)GetValue(IsScriptNotifyAllowedProperty);
            set => SetValue(IsScriptNotifyAllowedProperty, value);
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

        /// <inheritdoc />
        [Browsable(false)]
        public WebViewControlProcess Process
        {
            get
            {
                VerifyAccess();

                // NOTE: Not really in the spirit of a Property
                do
                {
                    Dispatcher.CurrentDispatcher.DoEvents();
                }
                while (!_initializationComplete.WaitOne(InitializationBlockingTime));

                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.Process;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <inheritdoc />
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

        /// <inheritdoc cref="IWebView.Close" />
        public override void Close()
        {
            // TODO: Guard IsDisposed
            UnsubscribeEvents();
            _webViewControl?.Close();
            _webViewControl?.Dispose();

            _webViewControl = null;
            _process = null;
        }

        /// <inheritdoc />
        public WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GetDeferredPermissionRequestById(id);
        }

        /// <inheritdoc />
        public bool GoBack()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GoBack() ?? false;
        }

        /// <inheritdoc />
        public bool GoForward()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.GoForward() ?? false;
        }

        /// <inheritdoc />
        public string InvokeScript(string scriptName)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName);
        }

        /// <inheritdoc />
        public string InvokeScript(string scriptName, params string[] arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName, arguments);
        }

        /// <inheritdoc />
        public string InvokeScript(string scriptName, IEnumerable<string> arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScript(scriptName, arguments);
        }

        /// <inheritdoc />
        public Task<string> InvokeScriptAsync(string scriptName)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName);
        }

        /// <inheritdoc />
        public Task<string> InvokeScriptAsync(string scriptName, params string[] arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName, arguments);
        }

        /// <inheritdoc />
        public Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            return _webViewControl?.InvokeScriptAsync(scriptName, arguments);
        }

        /// <inheritdoc />
        public void MoveFocus(WebViewControlMoveFocusReason reason)
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.MoveFocus(reason);
        }

        /// <inheritdoc />
        public void Navigate(string source)
        {
            Navigate(UriHelper.StringToUri(source));
        }

        /// <inheritdoc />
        public void Navigate(Uri source)
        {
            VerifyAccess();

            // TODO: Support for pack://
            Source = source;
        }

        /// <inheritdoc />
        public void NavigateToLocal(string relativePath)
        {
            VerifyAccess();

            do
            {
                Dispatcher.CurrentDispatcher.DoEvents();
            }
            while (!_initializationComplete.WaitOne(InitializationBlockingTime));

            Verify.IsNotNull(_webViewControl);
            _webViewControl.NavigateToLocal(relativePath);
        }

        /// <inheritdoc />
        public void NavigateToString(string text)
        {
            VerifyAccess();

            do
            {
                Dispatcher.CurrentDispatcher.DoEvents();
            }
            while (!_initializationComplete.WaitOne(InitializationBlockingTime));

            Verify.IsNotNull(_webViewControl);
            _webViewControl.NavigateToString(text);
        }

        /// <inheritdoc />
        public void Refresh()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Refresh();
        }

        /// <inheritdoc />
        public void Stop()
        {
            VerifyAccess();
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Stop();
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            OSVersionHelper.ThrowIfBeforeWindows10April2018();

            DpiHelper.SetPerMonitorDpiAwareness();

            Verify.AreEqual(_initializationState, InitializationState.IsInitializing);

            if (_process == null)
            {
                var privateNetworkEnabled = !Dispatcher.CheckAccess()
                    ? Dispatcher.Invoke(() => IsPrivateNetworkClientServerCapabilityEnabled)
                    : IsPrivateNetworkClientServerCapabilityEnabled;
                var enterpriseId = !Dispatcher.CheckAccess()
                    ? Dispatcher.Invoke(() => EnterpriseId)
                    : EnterpriseId;

                _process = new WebViewControlProcess(new WebViewControlProcessOptions
                {
                    PrivateNetworkClientServerCapability = privateNetworkEnabled
                        ? WebViewControlProcessCapabilityState.Enabled
                        : WebViewControlProcessCapabilityState.Disabled,
                    EnterpriseId = enterpriseId
                });
            }

            Dispatcher.InvokeAsync(
                async () =>
                {
                    Verify.IsNotNull(_process);

                    if (_webViewControl == null)
                    {
                        var handle = ChildWindow.Handle;
                        var bounds = new Windows.Foundation.Rect(0, 0, RenderSize.Width, RenderSize.Height);

                        _webViewControl = await _process.CreateWebViewControlHostAsync(handle, bounds).ConfigureAwait(false);
                    }

                    Verify.IsNotNull(_webViewControl);

                    UpdateSize(RenderSize);

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

                    _initializationState = InitializationState.IsInitialized;
                    _initializationComplete.Set();
                },
                DispatcherPriority.Send);
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override void UpdateBounds(Rect bounds)
        {
            UpdateBounds((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height);
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

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is WebView wv)
            {
                if (dependencyPropertyChangedEventArgs.Property.Name == nameof(Source))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        wv._webViewControl.Navigate(dependencyPropertyChangedEventArgs.NewValue as Uri);
                    }
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsIndexedDBEnabled))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        wv._webViewControl.Settings.IsIndexedDBEnabled =
                            (bool)dependencyPropertyChangedEventArgs.NewValue;
                    }
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsJavaScriptEnabled))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        wv._webViewControl.Settings.IsJavaScriptEnabled =
                            (bool)dependencyPropertyChangedEventArgs.NewValue;
                    }
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsScriptNotifyAllowed))
                {
                    Verify.IsTrue(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        wv._webViewControl.Settings.IsScriptNotifyAllowed =
                            (bool)dependencyPropertyChangedEventArgs.NewValue;
                    }
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(IsPrivateNetworkClientServerCapabilityEnabled))
                {
                    Verify.IsFalse(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        throw new InvalidOperationException(DesignerUI.E_CANNOT_CHANGE_AFTER_INIT);
                    }
                }
                else if (dependencyPropertyChangedEventArgs.Property.Name == nameof(EnterpriseId))
                {
                    Verify.IsFalse(wv.WebViewControlInitialized);
                    if (wv.WebViewControlInitialized)
                    {
                        throw new InvalidOperationException(DesignerUI.E_CANNOT_CHANGE_AFTER_INIT);
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

        private void UpdateBounds(int x, int y, int width, int height)
        {
#if DEBUG_LAYOUT
            Debug.WriteLine($"{Name}::{nameof(UpdateBounds)}");
            Debug.Indent();
            Debug.WriteLine($"oldBounds={{x={x} y={y} width={width} height={height}}}");
#endif

            // Update bounds here to ensure correct draw position
            if (IsScalingRequired)
            {
                width = DpiHelper.LogicalToDeviceUnits(width, DeviceDpi);
                height = DpiHelper.LogicalToDeviceUnits(height, DeviceDpi);
            }

            // HACK: looks like the vertical pos is counted twice, giving a gap
            y = 0;

#if DEBUG_LAYOUT
            Debug.WriteLine($"newBounds={{x={x} y={y} width={width} height={height}}}");
#endif
            if (WebViewControlInitialized)
            {
                var rect = new Windows.Foundation.Rect(
                    new Point(x, y),
                    new Size(width, height));

                _webViewControl?.UpdateBounds(rect);
            }

#if DEBUG_LAYOUT
            Debug.Unindent();
#endif
        }
    }
}