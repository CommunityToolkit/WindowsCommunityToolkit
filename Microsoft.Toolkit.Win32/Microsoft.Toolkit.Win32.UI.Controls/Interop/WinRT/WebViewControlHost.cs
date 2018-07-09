// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading.Tasks;

using Windows.Web;
using Windows.Web.UI;
using Windows.Web.UI.Interop;

using Rect = Windows.Foundation.Rect;

// Suppress document warnings as the items are internal and are used to propagate exception info up to consuming classes
#pragma warning disable SA1604 // Element documentation must have summary
#pragma warning disable SA1615 // Element return value must be documented

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Provides a control that hosts HTML content in an app.
    /// </summary>
    /// <remarks>
    /// <see cref="WebViewControlHost"/> navigation events occur in the following order:
    /// <list type="bullet">
    /// <item><see cref="NavigationStarting"/></item>
    /// <item><see cref="ContentLoading"/></item>
    /// <item><see cref="DOMContentLoaded"/></item>
    /// <item><see cref="NavigationCompleted"/></item>
    /// </list>
    /// Similar events occur in the same order for each iframe in the <see cref="WebViewControlHost"/> content:
    /// <list type="bullet">
    /// <item><see cref="FrameNavigationStarting"/></item>
    /// <item><see cref="FrameContentLoading"/></item>
    /// <item><see cref="FrameDOMContentLoaded"/></item>
    /// <item><see cref="FrameNavigationCompleted"/></item>
    /// </list>
    /// </remarks>
    internal sealed class WebViewControlHost : IDisposable
    {
        [SecurityCritical]
        private WebViewControl _webViewControl;

        private bool _webViewControlClosed;

        internal WebViewControlHost(WebViewControl webViewControl)
        {
            Verify.IsNotNull(webViewControl);

            _webViewControl = webViewControl ?? throw new ArgumentNullException(nameof(webViewControl));
            Process = _webViewControl.Process;
            SubscribeEvents();
            SubscribeProcessExited();
        }

        ~WebViewControlHost()
        {
            Dispose(false);
        }

        internal event EventHandler<WebViewControlAcceleratorKeyPressedEventArgs> AcceleratorKeyPressed = (sender, args) => { };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "This is the declaration from WinRT")]
        internal event EventHandler<object> ContainsFullScreenElementChanged = (sender, args) => { };

        internal event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading = (sender, args) => { };

        internal event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded = (sender, args) => { };

        internal event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading = (sender, args) => { };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DOM", Justification = "This is the name from WinRT")]
        internal event EventHandler<WebViewControlDOMContentLoadedEventArgs> FrameDOMContentLoaded = (sender, args) => { };

        internal event EventHandler<WebViewControlNavigationCompletedEventArgs> FrameNavigationCompleted = (sender, args) => { };

        internal event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting = (sender, args) => { };

        internal event EventHandler<WebViewControlLongRunningScriptDetectedEventArgs> LongRunningScriptDetected = (sender, args) => { };

        internal event EventHandler<WebViewControlMoveFocusRequestedEventArgs> MoveFocusRequested = (sender, args) => { };

        internal event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted = (sender, args) => { };

        internal event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting = (sender, args) => { };

        internal event EventHandler<WebViewControlNewWindowRequestedEventArgs> NewWindowRequested = (sender, args) => { };

        internal event EventHandler<WebViewControlPermissionRequestedEventArgs> PermissionRequested = (sender, args) => { };

        internal event EventHandler<WebViewControlScriptNotifyEventArgs> ScriptNotify = (sender, args) => { };

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1009:DeclareEventHandlersCorrectly",
            Justification = "WinRT type does not derive from EventArgs. Signature kept to maintain compatibility")]
        internal event EventHandler<object> UnsafeContentWarningDisplaying = (sender, args) => { };

        internal event EventHandler<WebViewControlUnsupportedUriSchemeIdentifiedEventArgs> UnsupportedUriSchemeIdentified = (sender, args) => { };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable", Justification ="This is the name from WinRT")]
        internal event EventHandler<WebViewControlUnviewableContentIdentifiedEventArgs> UnviewableContentIdentified = (sender, args) => { };

        internal static bool IsSupported => OSVersionHelper.IsWindows10April2018OrGreater
                                            && OSVersionHelper.IsWorkstation
                                            && OSVersionHelper.EdgeExists;

        internal bool CanGoBack
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (!IsDisposed && _webViewControl != null)
                {
                    return _webViewControl.CanGoBack;
                }

                return false;
            }
        }

        internal bool CanGoForward
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (!IsDisposed && _webViewControl != null)
                {
                    return _webViewControl.CanGoForward;
                }

                return false;
            }
        }

        internal bool ContainsFullScreenElement
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (!IsDisposed && _webViewControl != null)
                {
                    return _webViewControl.ContainsFullScreenElement;
                }

                return false;
            }
        }

        internal string DocumentTitle
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                return _webViewControl?.DocumentTitle;
            }
        }

        private bool IsDisposed { get; set; }

        private bool IsDisposing { get; set; }

        internal bool IsVisible
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                return _webViewControl?.IsVisible ?? false;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                _webViewControl.IsVisible = value;
            }
        }

        internal Windows.Web.UI.Interop.WebViewControlProcess Process { get; private set; }

        internal Windows.Web.UI.WebViewControlSettings Settings
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);
                return _webViewControl?.Settings;
            }
        }

        internal Uri Source
        {
            get
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                Uri url = null;

                if (_webViewControl != null)
                {
                    // Current url
                    url = _webViewControl.Source;

                    // When Source set to null or navigating to stream/string, we navigate to "about:blank"
                    // internally. Make sure we return null in those cases.
                    // Note that the current Source may not be "about:blank" yet
                    // Also, we'll (inconsistently) return "about:blank" in some cases
                    if (NavigatingToAboutBlank)
                    {
                        url = null;
                    }
                }

                return url;
            }

            set
            {
                Verify.IsFalse(IsDisposed);
                Verify.IsNotNull(_webViewControl);

                if (_webViewControl != null)
                {
                    Navigate(value);
                }
            }
        }

        /// <summary>
        /// Gets the version of EDGEHTML.DLL used by the control.
        /// </summary>
        /// <value>The version of EDGEHTML.DLL used by the control.</value>
        internal Version Version
        {
            get
            {
                if (OSVersionHelper.EdgeExists)
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "edgehtml.dll"));
                    return new Version(
                        versionInfo.FileMajorPart,
                        versionInfo.FileMinorPart,
                        versionInfo.FileBuildPart,
                        versionInfo.FilePrivatePart);
                }

                // Reuse the message, close enough
                throw new InvalidOperationException(DesignerUI.E_NOTSUPPORTED_OS_RS4);
            }
        }

        private Guid LastNavigation
        {
            get;
            [SecurityCritical]
            set;
        }

        // Indicates whether we are navigating to "about:blank" internally because Source is set to null or navigating to string
        // Set is SecurityCritical because it is involved in making security decisions
        private bool NavigatingToAboutBlank
        {
            get;
            [SecurityCritical]
            set;
        }

        internal Uri BuildStream(string contentIdentifier, string relativePath)
        {
            return _webViewControl?.BuildLocalStreamUri(contentIdentifier, relativePath);
        }

        internal void Close()
        {
            var webViewControlAlreadyClosed = _webViewControlClosed;
            _webViewControlClosed = true;

            // Unsubscribe all events:
            UnsubscribeEvents();
            UnsubscribeProcessExited();

            if (!webViewControlAlreadyClosed)
            {
                Verify.IsNotNull(_webViewControl);

                _webViewControl?.Close();
            }

            _webViewControl = null;
            Process = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal WebViewControlDeferredPermissionRequest GetDeferredPermissionRequestById(uint id)
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);
            if (_webViewControl == null)
            {
                return null;
            }

            _webViewControl.GetDeferredPermissionRequestById(id, out var retval);
            return retval;
        }

        internal bool GoBack()
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);

            var retval = _webViewControl != null;
            try
            {
                _webViewControl?.GoBack();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }

                retval = false;
            }

            return retval;
        }

        internal bool GoForward()
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);

            var retval = _webViewControl != null;
            try
            {
                _webViewControl?.GoForward();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }

                retval = false;
            }

            return retval;
        }

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        internal string InvokeScript(string scriptName) => InvokeScript(scriptName, null);

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        internal string InvokeScript(string scriptName, params string[] arguments) => InvokeScript(scriptName, (IEnumerable<string>)arguments);

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        internal string InvokeScript(string scriptName, IEnumerable<string> arguments) => InvokeScriptAsync(scriptName, arguments).GetAwaiter().GetResult();

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        internal Task<string> InvokeScriptAsync(string scriptName) => InvokeScriptAsync(scriptName, null);

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        internal Task<string> InvokeScriptAsync(string scriptName, params string[] arguments) => InvokeScriptAsync(scriptName, (IEnumerable<string>)arguments);

        /// <exception cref="InvalidOperationException">When the underlying <see cref="WebViewControl"/> is not yet initialized.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scriptName"/> is <see langword="null"/></exception>
        internal Task<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);
            Verify.IsNeitherNullNorEmpty(scriptName);

            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentNullException(nameof(scriptName));
            }

            // Cannot invoke script
            if (_webViewControl == null)
            {
                throw new InvalidOperationException(DesignerUI.E_WEBVIEW_CANNOT_INVOKE_BEFORE_INIT);
            }

            // Protect against the cross domain scripting attacks
            // If it is our internal navigation to about:blank for navigating to null or load string or before navigation has happened, Source will be null
            var currentSource = Source;
            if (currentSource != null)
            {
                Security.DemandWebPermission(currentSource);
            }

            return _webViewControl
                    .InvokeScriptAsync(scriptName, arguments)
                    .AsTask();
        }

        internal void MoveFocus(WebViewControlMoveFocusReason reason)
        {
            _webViewControl?.MoveFocus((Windows.Web.UI.Interop.WebViewControlMoveFocusReason)reason);
        }

        /// <exception cref="ArgumentException">The provided <paramref name="source"/> is a relative URI.</exception>
        internal void Navigate(Uri source)
        {
            Verify.IsNotNull(_webViewControl);

            if (_webViewControl != null)
            {
                // Cancel any outstanding navigation
                // TODO: Does this show a cancel page? Can we suppress that?
                _webViewControl.Stop();

                LastNavigation = Guid.NewGuid();

                if (source == null)
                {
                    NavigatingToAboutBlank = true;
                    source = WebViewDefaults.AboutBlankUri;
                }
                else
                {
                    CleanInternalState();
                }

                // Absolute URI only. Not sure what the host would be if using relative
                if (!source.IsAbsoluteUri)
                {
                    throw new ArgumentException(DesignerUI.E_WEBVIEW_NOT_ABSOLUTE_URI, nameof(source));
                }

                // TODO: Handle POPUP window
                // TODO: Handle navigation for frame

                // TODO: Security for partial trust (e.g. about:blank is not allowed)
                // If we are navigating to "about:blank" internally as a result of setting source to null
                // or navigating to string, do not demand WebPermission
                if (!NavigatingToAboutBlank)
                {
                    Security.DemandWebPermission(source);
                }

                // TODO: Sanitize URI containing invalid UTF-8 sequences
                try
                {
                    _webViewControl.Navigate(source);
                }
                catch (Exception)
                {
                    // Clear internal state if navigation fails
                    CleanInternalState();

                    throw;
                }
            }
        }

        /// <exception cref="UriFormatException">
        ///                 In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
        ///               <paramref name="source" /> is empty.-or- The scheme specified in <paramref name="source" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.-or-
        ///               <paramref name="source" /> contains too many slashes.-or- The password specified in <paramref name="source" /> is not valid.-or- The host name specified in <paramref name="source" /> is not valid.-or- The file name specified in <paramref name="source" /> is not valid. -or- The user name specified in <paramref name="source" /> is not valid.-or- The host or authority name specified in <paramref name="source" /> cannot be terminated by backslashes.-or- The port number specified in <paramref name="source" /> is not valid or cannot be parsed.-or- The length of <paramref name="source" /> exceeds 65519 characters.-or- The length of the scheme specified in <paramref name="source" /> exceeds 1023 characters.-or- There is an invalid character sequence in <paramref name="source" />.-or- The MS-DOS path specified in <paramref name="source" /> must start with c:\\.</exception>
        internal void Navigate(string source)
        {
            Navigate(UriHelper.StringToUri(source));
        }

        internal void NavigateToLocal(string relativePath)
        {
            var uri = BuildStream("LocalContent", relativePath);
            var resolver = new UriToLocalStreamResolver();
            NavigateToLocalStreamUri(uri, resolver);
        }

        internal void NavigateToLocalStreamUri(Uri source, IUriToStreamResolver streamResolver)
        {
            _webViewControl?.NavigateToLocalStreamUri(source, streamResolver);
        }

        /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null"/></exception>
        internal void NavigateToString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            _webViewControl?.NavigateToString(text);
        }

        internal void Refresh()
        {
            try
            {
                _webViewControl?.Refresh();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }
            }
        }

        internal void Stop()
        {
            try
            {
                _webViewControl?.Stop();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }
            }
        }

        // We don't expose the Bounds property because of issues with placement of Core window
        internal void UpdateBounds(Rect bounds)
        {
            if (_webViewControl != null)
            {
                _webViewControl.Bounds = bounds;
            }
        }

        [SecurityCritical] // Resets NavigatingToAboutBlank which is used in security decisions
        private void CleanInternalState()
        {
            NavigatingToAboutBlank = false;
        }

        private void Dispose(bool disposing)
        {
            IsDisposing = true;
            try
            {
                if (disposing)
                {
                    Close();
                }
            }
            finally
            {
                IsDisposing = false;
                IsDisposed = true;
            }
        }

        private void OnAcceleratorKeyPressed(WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            var handler = AcceleratorKeyPressed;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnAcceleratorKeyPressed(WebViewControl sender, Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args) => OnAcceleratorKeyPressed(args);

        private void OnContainsFullScreenElementChanged(object args)
        {
            var handler = ContainsFullScreenElementChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContainsFullScreenElementChanged(IWebViewControl sender, object args) => OnContainsFullScreenElementChanged(args);

        private void OnContentLoading(WebViewControlContentLoadingEventArgs args)
        {
            var handler = ContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnContentLoading(IWebViewControl sender, Windows.Web.UI.WebViewControlContentLoadingEventArgs args) => OnContentLoading(args);

        private void OnDOMContentLoaded(WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = DOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnDOMContentLoaded(IWebViewControl sender, Windows.Web.UI.WebViewControlDOMContentLoadedEventArgs args)
        {
            // When Source set to null or navigating to stream/string, we navigate to "about:blank" internally.
            if (NavigatingToAboutBlank)
            {
                Verify.Implies(NavigatingToAboutBlank, Source == null || Source == WebViewDefaults.AboutBlankUri);

                // Make sure we pass null in the event args
                var a = new WebViewControlDOMContentLoadedEventArgs((Uri)null);
                OnDOMContentLoaded(a);
            }
            else
            {
                OnDOMContentLoaded(args);
            }
        }

        private void OnFrameContentLoading(WebViewControlContentLoadingEventArgs args)
        {
            var handler = FrameContentLoading;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameContentLoading(IWebViewControl sender, Windows.Web.UI.WebViewControlContentLoadingEventArgs args) => OnFrameContentLoading(args);

        private void OnFrameDOMContentLoaded(WebViewControlDOMContentLoadedEventArgs args)
        {
            var handler = FrameDOMContentLoaded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameDOMContentLoaded(IWebViewControl sender, Windows.Web.UI.WebViewControlDOMContentLoadedEventArgs args) => OnFrameDOMContentLoaded(args);

        private void OnFrameNavigationCompleted(WebViewControlNavigationCompletedEventArgs args)
        {
            var handler = FrameNavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationCompleted(IWebViewControl sender, Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            // TODO: Need to handle frame navigation like NavigationCompleted?
            OnFrameNavigationCompleted(args);
        }

        private void OnFrameNavigationStarting(WebViewControlNavigationStartingEventArgs args)
        {
            var handler = FrameNavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnFrameNavigationStarting(IWebViewControl sender, Windows.Web.UI.WebViewControlNavigationStartingEventArgs args) => OnFrameNavigationStarting(args);

        private void OnLongRunningScriptDetected(WebViewControlLongRunningScriptDetectedEventArgs args)
        {
            var handler = LongRunningScriptDetected;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnLongRunningScriptDetected(IWebViewControl sender, Windows.Web.UI.WebViewControlLongRunningScriptDetectedEventArgs args) => OnLongRunningScriptDetected(args);

        private void OnMoveFocusRequested(WebViewControlMoveFocusRequestedEventArgs args)
        {
            var handler = MoveFocusRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnMoveFocusRequested(WebViewControl sender, Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args) => OnMoveFocusRequested(args);

        private void OnNavigationCompleted(WebViewControlNavigationCompletedEventArgs args)
        {
            // We could have used
            // if (NavigationCompleted != null) NavigationCompleted(this, args);
            // However, if there is a subscriber and the moment the null check and the call to
            // the event handler by the method is invoked, the subscriber may unsubscribe
            // (e.g. on a different thread) and cause a NullReferenceException.
            // To work around this create a temporarily local variable to store the reference and check that
            var handler = NavigationCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationCompleted(IWebViewControl sender, Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            // When Source set to null or navigating to stream/string, we navigate to "about:blank" internally.
            if (NavigatingToAboutBlank)
            {
                Verify.Implies(NavigatingToAboutBlank, Source == null || Source == WebViewDefaults.AboutBlankUri);

                // Make sure we pass null in the event args
                var a = new WebViewControlNavigationCompletedEventArgs(args, null);
                OnNavigationCompleted(a);
            }
            else
            {
                OnNavigationCompleted(args);
            }
        }

        private void OnNavigationStarting(WebViewControlNavigationStartingEventArgs args)
        {
            var handler = NavigationStarting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNavigationStarting(IWebViewControl sender, Windows.Web.UI.WebViewControlNavigationStartingEventArgs args)
        {
            var newNavigationRequested = false;
            var cancelRequested = false;

            try
            {
                var url = args.Uri;

                // The NavigatingToAboutBlank property indicates whether we are navigating to "about:blank" as a result of navigating
                // to a null source, or stream/string navigation.
                // We set the NavigatingToAboutBlank bit to true in the void Navigate(Uri) method. When the above conditions are true,
                // the NavigatingToAboutBlank is true and the source must be "about:blank"
                if (NavigatingToAboutBlank && url != null && url != new Uri("about:blank"))
                {
                    NavigatingToAboutBlank = false;
                }

                if (!NavigatingToAboutBlank && !Security.CallerHasWebPermission(url))
                {
                    cancelRequested = true;
                }
                else
                {
                    // When Source is set to null or navigating to stream/string, we navigate to "about:blank" internally.
                    // Make sure we pass null in the event args
                    if (NavigatingToAboutBlank)
                    {
                        url = null;
                    }

                    var a = new WebViewControlNavigationStartingEventArgs(args, url);

                    // Launching a navigation from the NavigationStarting event handler causes re-entrancy
                    var lastNavigation = LastNavigation;

                    // Fire navigating event
                    OnNavigationStarting(a);

                    if (LastNavigation != lastNavigation)
                    {
                        newNavigationRequested = true;
                    }

                    cancelRequested = a.Cancel;
                }
            }

            // Disable to suppress FXCop warning since we really do want to catch all exceptions
#pragma warning disable 6502
            catch
            {
                cancelRequested = true;
            }
#pragma warning restore 6502
            finally
            {
                if (cancelRequested && !newNavigationRequested)
                {
                    CleanInternalState();
                }

                if (cancelRequested || newNavigationRequested)
                {
                    args.Cancel = true;
                }
            }
        }

        private void OnNewWindowRequested(WebViewControlNewWindowRequestedEventArgs args)
        {
            var handler = NewWindowRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnNewWindowRequested(IWebViewControl sender, Windows.Web.UI.WebViewControlNewWindowRequestedEventArgs args) => OnNewWindowRequested(args);

        private void OnOnScriptNotify(IWebViewControl sender, Windows.Web.UI.WebViewControlScriptNotifyEventArgs args) => OnScriptNotify(args);

        private void OnPermissionRequested(WebViewControlPermissionRequestedEventArgs args)
        {
            var handler = PermissionRequested;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPermissionRequested(IWebViewControl sender, Windows.Web.UI.WebViewControlPermissionRequestedEventArgs args) => OnPermissionRequested(args);

        private void OnProcessExited(object sender, object e)
        {
            _webViewControlClosed = true;
            Close();
        }

        private void OnScriptNotify(WebViewControlScriptNotifyEventArgs args)
        {
            var handler = ScriptNotify;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsafeContentWarningDisplaying(object args)
        {
            var handler = UnsafeContentWarningDisplaying;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsafeContentWarningDisplaying(IWebViewControl sender, object args) => OnUnsafeContentWarningDisplaying(args);

        private void OnUnsupportedUriSchemeIdentified(WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            var handler = UnsupportedUriSchemeIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnsupportedUriSchemeIdentified(IWebViewControl sender, Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) => OnUnsupportedUriSchemeIdentified(args);

        private void OnUnviewableContentIdentified(WebViewControlUnviewableContentIdentifiedEventArgs args)
        {
            var handler = UnviewableContentIdentified;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnUnviewableContentIdentified(IWebViewControl sender, Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) => OnUnviewableContentIdentified(args);

        [SecurityCritical]
        private void SubscribeEvents()
        {
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
            _webViewControl.ScriptNotify += OnOnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying += OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified += OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified += OnUnviewableContentIdentified;
        }

        [SecurityCritical]
        private void SubscribeProcessExited()
        {
            if (Process == null)
            {
                return;
            }

            Process.ProcessExited += OnProcessExited;
        }

        private void UnsubscribeEvents()
        {
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
            _webViewControl.ScriptNotify -= OnOnScriptNotify;
            _webViewControl.UnsafeContentWarningDisplaying -= OnUnsafeContentWarningDisplaying;
            _webViewControl.UnsupportedUriSchemeIdentified -= OnUnsupportedUriSchemeIdentified;
            _webViewControl.UnviewableContentIdentified -= OnUnviewableContentIdentified;
        }

        private void UnsubscribeProcessExited()
        {
            if (Process == null)
            {
                return;
            }

            try
            {
                Process.ProcessExited -= OnProcessExited;
            }
            catch (Exception)
            {
                // Yes, really catch all
                // 'The process terminated unexpectedly. (Exception from HRESULT: 0x8007042B)'
            }
        }

        // TODO: Expose Bounds
    }
}